namespace EvaWoods.Application.Materiale;

public record PiesaDeAsezat(
    Guid PiesaId,
    string? Nume,
    decimal LungimeMm,
    decimal LatimeMm,
    bool PoateRoti);

public record PlasarePiesa(
    Guid PiesaId,
    string? Nume,
    decimal X,
    decimal Y,
    decimal LungimeMm,
    decimal LatimeMm,
    bool Rotita);

public record FoaieRezultat(
    int Numar,
    List<PlasarePiesa> Plasari,
    decimal UtilizareProcent);

public record RezultatOptimizare(
    int NumarFoi,
    List<FoaieRezultat> Foi,
    decimal UtilizareProcent,
    List<string> PieseNeplasabile);

public static class OptimizatorDebitare
{
    private record DreptunghiLiber(
        decimal X,
        decimal Y,
        decimal Latime,
        decimal Inaltime);

    private enum TipSplit
    {
        DeasupraPeToataLatimea,
        DreaptaPeToataInaltimea
    }

    private sealed class FoaieInLucru
    {
        public List<PlasarePiesa> Plasari { get; } = new();

        public List<DreptunghiLiber> Libere { get; } = new();

        public decimal SuprafataOcupataMm2 { get; set; }
    }

    private sealed record CandidatPlasare(
        FoaieInLucru Foaie,
        DreptunghiLiber Liber,
        bool Rotit,
        decimal LatimePiesa,
        decimal InaltimePiesa,
        TipSplit Split,
        decimal LongSideFit,
        decimal ShortSideFit,
        decimal SuprafataLiberaDupaSplit,
        decimal CelMaiMareRestMm2);

    /// <summary>
    /// Optimizeaza asezarea pieselor pe foi PAL folosind o strategie
    /// guillotine, potrivita pentru debitare cu circular / panel saw.
    ///
    /// Marginea de curatare este eliminata din toate cele 4 laturi.
    /// Kerf-ul este consumat la fiecare separare intre piesa si rest.
    /// </summary>
    public static RezultatOptimizare Optimizeaza(
        IReadOnlyList<PiesaDeAsezat> piese,
        decimal lungimeFoaieMm,
        decimal latimeFoaieMm,
        decimal kerfMm,
        decimal margineCuratareMm)
    {
        if (lungimeFoaieMm <= 0)
            throw new ArgumentOutOfRangeException(nameof(lungimeFoaieMm));

        if (latimeFoaieMm <= 0)
            throw new ArgumentOutOfRangeException(nameof(latimeFoaieMm));

        if (kerfMm < 0)
            throw new ArgumentOutOfRangeException(nameof(kerfMm));

        if (margineCuratareMm < 0)
            throw new ArgumentOutOfRangeException(nameof(margineCuratareMm));

        var latimeUtila =
            lungimeFoaieMm - 2 * margineCuratareMm;

        var inaltimeUtila =
            latimeFoaieMm - 2 * margineCuratareMm;

        if (latimeUtila <= 0 || inaltimeUtila <= 0)
        {
            throw new ArgumentException(
                "Marginea de curatare este prea mare pentru dimensiunea foii.");
        }

        var neplasabile = new List<string>();
        var deAsezat = new List<PiesaDeAsezat>();

        foreach (var piesa in piese)
        {
            if (piesa.LungimeMm <= 0 || piesa.LatimeMm <= 0)
            {
                neplasabile.Add(
                    $"{piesa.Nume ?? "Piesa"} are dimensiuni invalide.");

                continue;
            }

            var incapeNormal =
                piesa.LungimeMm <= latimeUtila &&
                piesa.LatimeMm <= inaltimeUtila;

            var incapeRotit =
                piesa.PoateRoti &&
                piesa.LatimeMm <= latimeUtila &&
                piesa.LungimeMm <= inaltimeUtila;

            if (!incapeNormal && !incapeRotit)
            {
                neplasabile.Add(
                    $"{piesa.Nume ?? "Piesa"} " +
                    $"{piesa.LungimeMm:0.#} x {piesa.LatimeMm:0.#} mm " +
                    $"depaseste placa utila " +
                    $"{latimeUtila:0.#} x {inaltimeUtila:0.#} mm");

                continue;
            }

            deAsezat.Add(piesa);
        }

        /*
         * IMPORTANT
         *
         * Nu sortam doar dupa suprafata.
         *
         * Piesele lungi sunt de obicei mai greu de asezat dupa
         * fragmentarea foii, asa ca le prioritizam.
         */
        deAsezat = deAsezat
            .OrderByDescending(
                p => Math.Max(p.LungimeMm, p.LatimeMm))
            .ThenByDescending(
                p => Math.Min(p.LungimeMm, p.LatimeMm))
            .ThenByDescending(
                p => p.LungimeMm * p.LatimeMm)
            .ToList();

        var foi = new List<FoaieInLucru>();

        foreach (var piesa in deAsezat)
        {
            if (IncearcaPlaseaza(
                    foi,
                    piesa,
                    kerfMm))
            {
                continue;
            }

            var foaieNoua = CreeazaFoaie(
                latimeUtila,
                inaltimeUtila);

            foi.Add(foaieNoua);

            var plasat = IncearcaPlaseaza(
                new List<FoaieInLucru> { foaieNoua },
                piesa,
                kerfMm);

            /*
             * In mod normal nu ar trebui sa ajungem aici deoarece
             * piesele imposibile au fost filtrate mai sus.
             */
            if (!plasat)
            {
                neplasabile.Add(
                    $"{piesa.Nume ?? "Piesa"} " +
                    $"{piesa.LungimeMm:0.#} x {piesa.LatimeMm:0.#} mm " +
                    $"nu a putut fi plasata.");
            }
        }

        var suprafataUtilaFoaieMm2 =
            latimeUtila * inaltimeUtila;

        var foiRezultat = foi
            .Select(
                (foaie, index) =>
                    new FoaieRezultat(
                        index + 1,
                        foaie.Plasari,
                        suprafataUtilaFoaieMm2 > 0
                            ? Math.Round(
                                foaie.SuprafataOcupataMm2 /
                                suprafataUtilaFoaieMm2 *
                                100,
                                1)
                            : 0))
            .ToList();

        var utilizareTotala =
            foi.Count > 0 &&
            suprafataUtilaFoaieMm2 > 0
                ? Math.Round(
                    foi.Sum(
                        f => f.SuprafataOcupataMm2) /
                    (foi.Count *
                     suprafataUtilaFoaieMm2) *
                    100,
                    1)
                : 0;

        return new RezultatOptimizare(
            foi.Count,
            foiRezultat,
            utilizareTotala,
            neplasabile);
    }

    private static FoaieInLucru CreeazaFoaie(
        decimal latimeUtila,
        decimal inaltimeUtila)
    {
        var foaie = new FoaieInLucru();

        foaie.Libere.Add(
            new DreptunghiLiber(
                0,
                0,
                latimeUtila,
                inaltimeUtila));

        return foaie;
    }

    private static bool IncearcaPlaseaza(
        List<FoaieInLucru> foi,
        PiesaDeAsezat piesa,
        decimal kerf)
    {
        CandidatPlasare? celMaiBun = null;

        foreach (var foaie in foi)
        {
            foreach (var liber in foaie.Libere)
            {
                /*
                 * Orientare normala
                 */
                EvalueazaOrientare(
                    foaie,
                    liber,
                    piesa.LungimeMm,
                    piesa.LatimeMm,
                    rotit: false,
                    kerf,
                    ref celMaiBun);

                /*
                 * Orientare rotita
                 */
                if (piesa.PoateRoti &&
                    piesa.LungimeMm != piesa.LatimeMm)
                {
                    EvalueazaOrientare(
                        foaie,
                        liber,
                        piesa.LatimeMm,
                        piesa.LungimeMm,
                        rotit: true,
                        kerf,
                        ref celMaiBun);
                }
            }
        }

        if (celMaiBun is null)
            return false;

        AplicaPlasare(
            celMaiBun,
            piesa,
            kerf);

        return true;
    }

    private static void EvalueazaOrientare(
        FoaieInLucru foaie,
        DreptunghiLiber liber,
        decimal latimePiesa,
        decimal inaltimePiesa,
        bool rotit,
        decimal kerf,
        ref CandidatPlasare? celMaiBun)
    {
        if (latimePiesa > liber.Latime ||
            inaltimePiesa > liber.Inaltime)
        {
            return;
        }

        var diferentaLatime =
            liber.Latime - latimePiesa;

        var diferentaInaltime =
            liber.Inaltime - inaltimePiesa;

        /*
         * Best Long Side Fit.
         *
         * Preferam plasarea care lasa cea mai mica
         * diferenta pe latura cea mai "slaba".
         */
        var longSideFit =
            Math.Max(
                diferentaLatime,
                diferentaInaltime);

        var shortSideFit =
            Math.Min(
                diferentaLatime,
                diferentaInaltime);

        /*
         * Testam ambele tipuri de split guillotine.
         */
        foreach (var split in Enum.GetValues<TipSplit>())
        {
            var resturi = CalculeazaResturi(
                liber,
                latimePiesa,
                inaltimePiesa,
                kerf,
                split);

            var suprafataLibera =
                resturi.Sum(
                    r => r.Latime * r.Inaltime);

            var celMaiMareRest =
                resturi.Count > 0
                    ? resturi.Max(
                        r => r.Latime * r.Inaltime)
                    : 0;

            var candidat =
                new CandidatPlasare(
                    foaie,
                    liber,
                    rotit,
                    latimePiesa,
                    inaltimePiesa,
                    split,
                    longSideFit,
                    shortSideFit,
                    suprafataLibera,
                    celMaiMareRest);

            if (EsteMaiBun(
                    candidat,
                    celMaiBun))
            {
                celMaiBun = candidat;
            }
        }
    }

    private static bool EsteMaiBun(
        CandidatPlasare candidat,
        CandidatPlasare? actual)
    {
        if (actual is null)
            return true;

        /*
         * 1. Minimizam latura ramasa cea mai mare.
         */
        if (candidat.LongSideFit <
            actual.LongSideFit)
        {
            return true;
        }

        if (candidat.LongSideFit >
            actual.LongSideFit)
        {
            return false;
        }

        /*
         * 2. La egalitate, minimizam si restul
         *    de pe cealalta latura.
         */
        if (candidat.ShortSideFit <
            actual.ShortSideFit)
        {
            return true;
        }

        if (candidat.ShortSideFit >
            actual.ShortSideFit)
        {
            return false;
        }

        /*
         * 3. Preferam split-ul care conserva
         *    mai multa suprafata utilizabila.
         */
        if (candidat.SuprafataLiberaDupaSplit >
            actual.SuprafataLiberaDupaSplit)
        {
            return true;
        }

        if (candidat.SuprafataLiberaDupaSplit <
            actual.SuprafataLiberaDupaSplit)
        {
            return false;
        }

        /*
         * 4. Daca si asta este egal,
         *    preferam un rest mare continuu
         *    in loc de multe fragmente mici.
         */
        if (candidat.CelMaiMareRestMm2 >
            actual.CelMaiMareRestMm2)
        {
            return true;
        }

        return false;
    }

    private static void AplicaPlasare(
        CandidatPlasare candidat,
        PiesaDeAsezat piesa,
        decimal kerf)
    {
        var foaie = candidat.Foaie;
        var liber = candidat.Liber;

        foaie.Plasari.Add(
            new PlasarePiesa(
                piesa.PiesaId,
                piesa.Nume,
                liber.X,
                liber.Y,
                candidat.LatimePiesa,
                candidat.InaltimePiesa,
                candidat.Rotit));

        foaie.SuprafataOcupataMm2 +=
            candidat.LatimePiesa *
            candidat.InaltimePiesa;

        foaie.Libere.Remove(liber);

        var resturi =
            CalculeazaResturi(
                liber,
                candidat.LatimePiesa,
                candidat.InaltimePiesa,
                kerf,
                candidat.Split);

        foreach (var rest in resturi)
        {
            if (rest.Latime > 0 &&
                rest.Inaltime > 0)
            {
                foaie.Libere.Add(rest);
            }
        }

        /*
         * Eliminam eventualele dreptunghiuri
         * complet continute in altele.
         */
        EliminaLibereRedundante(
            foaie.Libere);
    }

    private static List<DreptunghiLiber> CalculeazaResturi(
        DreptunghiLiber liber,
        decimal latimePiesa,
        decimal inaltimePiesa,
        decimal kerf,
        TipSplit split)
    {
        var rezultate =
            new List<DreptunghiLiber>();

        var latimeRamasa =
            liber.Latime -
            latimePiesa -
            kerf;

        var inaltimeRamasa =
            liber.Inaltime -
            inaltimePiesa -
            kerf;

        switch (split)
        {
            /*
             * +---------------------------+
             * |                           |
             * |        DEASUPRA           |
             * |                           |
             * +-------------+-------------+
             * |    PIESA    |   DREAPTA   |
             * +-------------+-------------+
             */
            case TipSplit.DeasupraPeToataLatimea:
            {
                if (latimeRamasa > 0)
                {
                    rezultate.Add(
                        new DreptunghiLiber(
                            liber.X +
                            latimePiesa +
                            kerf,
                            liber.Y,
                            latimeRamasa,
                            inaltimePiesa));
                }

                if (inaltimeRamasa > 0)
                {
                    rezultate.Add(
                        new DreptunghiLiber(
                            liber.X,
                            liber.Y +
                            inaltimePiesa +
                            kerf,
                            liber.Latime,
                            inaltimeRamasa));
                }

                break;
            }

            /*
             * +-------------+-------------+
             * |  DEASUPRA   |             |
             * |             |             |
             * +-------------+   DREAPTA   |
             * |    PIESA    |             |
             * +-------------+-------------+
             */
            case TipSplit.DreaptaPeToataInaltimea:
            {
                if (latimeRamasa > 0)
                {
                    rezultate.Add(
                        new DreptunghiLiber(
                            liber.X +
                            latimePiesa +
                            kerf,
                            liber.Y,
                            latimeRamasa,
                            liber.Inaltime));
                }

                if (inaltimeRamasa > 0)
                {
                    rezultate.Add(
                        new DreptunghiLiber(
                            liber.X,
                            liber.Y +
                            inaltimePiesa +
                            kerf,
                            latimePiesa,
                            inaltimeRamasa));
                }

                break;
            }
        }

        return rezultate;
    }

    private static void EliminaLibereRedundante(
        List<DreptunghiLiber> libere)
    {
        for (var i = libere.Count - 1; i >= 0; i--)
        {
            for (var j = 0; j < libere.Count; j++)
            {
                if (i == j)
                    continue;

                if (EsteInclus(
                        libere[i],
                        libere[j]))
                {
                    libere.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private static bool EsteInclus(
        DreptunghiLiber mic,
        DreptunghiLiber mare)
    {
        return
            mic.X >= mare.X &&
            mic.Y >= mare.Y &&
            mic.X + mic.Latime <=
            mare.X + mare.Latime &&
            mic.Y + mic.Inaltime <=
            mare.Y + mare.Inaltime;
    }
}