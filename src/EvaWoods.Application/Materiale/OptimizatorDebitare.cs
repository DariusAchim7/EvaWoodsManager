namespace EvaWoods.Application.Materiale;

public record PiesaDeAsezat(Guid PiesaId, string? Nume, decimal LungimeMm, decimal LatimeMm, bool PoateRoti);

public record PlasarePiesa(Guid PiesaId, string? Nume, decimal X, decimal Y, decimal LungimeMm, decimal LatimeMm, bool Rotita);

public record FoaieRezultat(int Numar, List<PlasarePiesa> Plasari, decimal UtilizareProcent);

public record RezultatOptimizare(
    int NumarFoi,
    List<FoaieRezultat> Foi,
    decimal UtilizareProcent,
    List<string> PieseNeplasabile);

public static class OptimizatorDebitare
{
    private record DreptunghiLiber(decimal X, decimal Y, decimal Latime, decimal Inaltime);

    private class FoaieInLucru
    {
        public List<PlasarePiesa> Plasari { get; } = new();
        public List<DreptunghiLiber> Libere { get; } = new();
        public decimal SuprafataOcupataMm2 { get; set; }
    }

    /// <summary>
    /// Așază piesele pe foi folosind algoritmul guillotine (tăieturi drepte, ca la panel saw).
    /// Marginea de curățare se scade de pe fiecare latură a foii; kerf-ul se consumă la fiecare tăietură.
    /// </summary>
    public static RezultatOptimizare Optimizeaza(
        IReadOnlyList<PiesaDeAsezat> piese,
        decimal lungimeFoaieMm,
        decimal latimeFoaieMm,
        decimal kerfMm,
        decimal margineCuratareMm)
    {
        var latimeUtila = lungimeFoaieMm - 2 * margineCuratareMm;
        var inaltimeUtila = latimeFoaieMm - 2 * margineCuratareMm;

        var neplasabile = new List<string>();
        var deAsezat = new List<PiesaDeAsezat>();

        foreach (var piesa in piese)
        {
            var incapeNormal = piesa.LungimeMm <= latimeUtila && piesa.LatimeMm <= inaltimeUtila;
            var incapeRotit = piesa.PoateRoti && piesa.LatimeMm <= latimeUtila && piesa.LungimeMm <= inaltimeUtila;

            if (!incapeNormal && !incapeRotit)
            {
                neplasabile.Add($"{piesa.Nume ?? "Piesă"} {piesa.LungimeMm:0.#} × {piesa.LatimeMm:0.#} mm depășește placa utilă {latimeUtila:0.#} × {inaltimeUtila:0.#} mm");
                continue;
            }

            deAsezat.Add(piesa);
        }

        // Sortare descrescătoare după suprafață - piesele mari primele
        deAsezat = deAsezat.OrderByDescending(p => p.LungimeMm * p.LatimeMm).ToList();

        var foi = new List<FoaieInLucru>();

        foreach (var piesa in deAsezat)
        {
            if (!IncearcaPlaseaza(foi, piesa, kerfMm))
            {
                var foaieNoua = new FoaieInLucru();
                foaieNoua.Libere.Add(new DreptunghiLiber(0, 0, latimeUtila, inaltimeUtila));
                foi.Add(foaieNoua);

                IncearcaPlaseaza(new List<FoaieInLucru> { foaieNoua }, piesa, kerfMm);
            }
        }

        var suprafataUtilaFoaieMm2 = latimeUtila * inaltimeUtila;
        var foiRezultat = foi.Select((foaie, index) => new FoaieRezultat(
            index + 1,
            foaie.Plasari,
            suprafataUtilaFoaieMm2 > 0 ? Math.Round(foaie.SuprafataOcupataMm2 / suprafataUtilaFoaieMm2 * 100, 1) : 0)).ToList();

        var utilizareTotala = foi.Count > 0 && suprafataUtilaFoaieMm2 > 0
            ? Math.Round(foi.Sum(f => f.SuprafataOcupataMm2) / (foi.Count * suprafataUtilaFoaieMm2) * 100, 1)
            : 0;

        return new RezultatOptimizare(foi.Count, foiRezultat, utilizareTotala, neplasabile);
    }

    private static bool IncearcaPlaseaza(List<FoaieInLucru> foi, PiesaDeAsezat piesa, decimal kerf)
    {
        FoaieInLucru? celMaiBunFoaie = null;
        DreptunghiLiber? celMaiBunLiber = null;
        var celMaiBunRotit = false;
        var celMaiBunScor = decimal.MaxValue;

        foreach (var foaie in foi)
        {
            foreach (var liber in foaie.Libere)
            {
                // Orientare normală
                if (piesa.LungimeMm <= liber.Latime && piesa.LatimeMm <= liber.Inaltime)
                {
                    var scor = Math.Min(liber.Latime - piesa.LungimeMm, liber.Inaltime - piesa.LatimeMm);
                    if (scor < celMaiBunScor)
                    {
                        celMaiBunScor = scor;
                        celMaiBunFoaie = foaie;
                        celMaiBunLiber = liber;
                        celMaiBunRotit = false;
                    }
                }

                // Orientare rotită 90°
                if (piesa.PoateRoti && piesa.LatimeMm <= liber.Latime && piesa.LungimeMm <= liber.Inaltime)
                {
                    var scor = Math.Min(liber.Latime - piesa.LatimeMm, liber.Inaltime - piesa.LungimeMm);
                    if (scor < celMaiBunScor)
                    {
                        celMaiBunScor = scor;
                        celMaiBunFoaie = foaie;
                        celMaiBunLiber = liber;
                        celMaiBunRotit = true;
                    }
                }
            }
        }

        if (celMaiBunFoaie is null || celMaiBunLiber is null) return false;

        var latimePiesa = celMaiBunRotit ? piesa.LatimeMm : piesa.LungimeMm;
        var inaltimePiesa = celMaiBunRotit ? piesa.LungimeMm : piesa.LatimeMm;

        celMaiBunFoaie.Plasari.Add(new PlasarePiesa(
            piesa.PiesaId, piesa.Nume,
            celMaiBunLiber.X, celMaiBunLiber.Y,
            latimePiesa, inaltimePiesa, celMaiBunRotit));

        celMaiBunFoaie.SuprafataOcupataMm2 += latimePiesa * inaltimePiesa;
        celMaiBunFoaie.Libere.Remove(celMaiBunLiber);

        // Împărțire guillotine: dreptunghi la dreapta + dreptunghi deasupra (cu kerf consumat)
        var latimeRamasa = celMaiBunLiber.Latime - latimePiesa - kerf;
        var inaltimeRamasa = celMaiBunLiber.Inaltime - inaltimePiesa - kerf;

        if (latimeRamasa > 0)
        {
            celMaiBunFoaie.Libere.Add(new DreptunghiLiber(
                celMaiBunLiber.X + latimePiesa + kerf,
                celMaiBunLiber.Y,
                latimeRamasa,
                inaltimePiesa));
        }

        if (inaltimeRamasa > 0)
        {
            celMaiBunFoaie.Libere.Add(new DreptunghiLiber(
                celMaiBunLiber.X,
                celMaiBunLiber.Y + inaltimePiesa + kerf,
                celMaiBunLiber.Latime,
                inaltimeRamasa));
        }

        return true;
    }
}