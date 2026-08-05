namespace EvaWoods.Domain.Proiecte;

public enum StatusProiect
{
    OfertaTrimisa,
    Masuratori,
    Proiectare,
    InProductie,
    LaMontaj,
    Finalizat
}

public enum TipPrincipalProiect
{
    Bucatarie,
    Dressing,
    MobilierBaie,
    Living,
    Dormitor,
    Birou,
    MobilierComercial,
    AltTip
}

public class Proiect
{
    public Guid Id { get; private set; }
    public Guid ClientId { get; private set; }
    public string CodProiect { get; private set; } = string.Empty;
    public string Nume { get; private set; } = string.Empty;
    public string? Subtip { get; private set; }
    public TipPrincipalProiect? TipPrincipal { get; private set; }
    public string? Descriere { get; private set; }
    public StatusProiect Status { get; private set; }
    public decimal Valoare { get; private set; }
    public DateTime? TermenLimita { get; private set; }
    public int Progres { get; private set; }
    public string? AdresaMontaj { get; private set; }
    public string? Note { get; private set; }
    public DateTime DataCreare { get; private set; }
    public string? ObservatiiClient { get; private set; }
    public string? Etaj { get; private set; }
    public bool? AreLift { get; private set; }
    public string? AccesAuto { get; private set; }
    public string? LocParcare { get; private set; }
    public string? PersoanaContactMontaj { get; private set; }
    public string? IntervalOrarPreferat { get; private set; }
    public string? ObservatiiTransport { get; private set; }
    public string? DescriereUrmatorulPas { get; private set; }
    public DateTime? TermenUrmatorulPas { get; private set; }
    public decimal? BugetAlocat { get; private set; }
    public decimal? ProcentManopera { get; private set; }
    public decimal? AvansProcent { get; private set; }
    public int? TermenExecutieZile { get; private set; }
    public int? ValabilitateOfertaZile { get; private set; }

    private Proiect() { }

    public static Proiect Creeaza(
        Guid clientId,
        string codProiect,
        string nume,
        decimal valoare,
        string? subtip = null,
        TipPrincipalProiect? tipPrincipal = null,
        string? descriere = null,
        DateTime? termenLimita = null,
        StatusProiect status = StatusProiect.OfertaTrimisa,
        string? adresaMontaj = null,
        decimal? bugetAlocat = null,
        decimal? procentManopera = null)
    {
        if (clientId == Guid.Empty)
            throw new ArgumentException("Clientul este obligatoriu.", nameof(clientId));
        if (string.IsNullOrWhiteSpace(nume))
            throw new ArgumentException("Numele proiectului este obligatoriu.", nameof(nume));
        if (valoare < 0)
            throw new ArgumentException("Valoarea nu poate fi negativă.", nameof(valoare));

        return new Proiect
        {
            Id = Guid.NewGuid(),
            ClientId = clientId,
            CodProiect = codProiect,
            Nume = nume.Trim(),
            Subtip = string.IsNullOrWhiteSpace(subtip) ? null : subtip.Trim(),
            TipPrincipal = tipPrincipal,
            Descriere = string.IsNullOrWhiteSpace(descriere) ? null : descriere.Trim(),
            Valoare = valoare,
            TermenLimita = termenLimita,
            Status = status,
            AdresaMontaj = string.IsNullOrWhiteSpace(adresaMontaj) ? null : adresaMontaj.Trim(),
            BugetAlocat = bugetAlocat,
            Progres = 0,
            DataCreare = DateTime.UtcNow,
            ProcentManopera = procentManopera
        };
    }

    public void Actualizeaza(
        string nume,
        decimal valoare,
        string? subtip,
        TipPrincipalProiect? tipPrincipal,
        string? descriere,
        DateTime? termenLimita,
        string? adresaMontaj,
        decimal? bugetAlocat,
        decimal? procentManopera)
    {
        if (string.IsNullOrWhiteSpace(nume))
            throw new ArgumentException("Numele proiectului este obligatoriu.", nameof(nume));
        if (valoare < 0)
            throw new ArgumentException("Valoarea nu poate fi negativă.", nameof(valoare));

        Nume = nume.Trim();
        Valoare = valoare;
        Subtip = string.IsNullOrWhiteSpace(subtip) ? null : subtip.Trim();
        TipPrincipal = tipPrincipal;
        Descriere = string.IsNullOrWhiteSpace(descriere) ? null : descriere.Trim();
        TermenLimita = termenLimita;
        AdresaMontaj = string.IsNullOrWhiteSpace(adresaMontaj) ? null : adresaMontaj.Trim();
        BugetAlocat = bugetAlocat;
        ProcentManopera = procentManopera;
    }

    public void ActualizeazaObservatiiClient(string? observatiiClient)
    {
        ObservatiiClient = string.IsNullOrWhiteSpace(observatiiClient) ? null : observatiiClient.Trim();
    }

    public void ActualizeazaDateMontaj(
        string? etaj,
        bool? areLift,
        string? accesAuto,
        string? locParcare,
        string? persoanaContactMontaj,
        string? intervalOrarPreferat,
        string? observatiiTransport)
    {
        Etaj = string.IsNullOrWhiteSpace(etaj) ? null : etaj.Trim();
        AreLift = areLift;
        AccesAuto = string.IsNullOrWhiteSpace(accesAuto) ? null : accesAuto.Trim();
        LocParcare = string.IsNullOrWhiteSpace(locParcare) ? null : locParcare.Trim();
        PersoanaContactMontaj = string.IsNullOrWhiteSpace(persoanaContactMontaj) ? null : persoanaContactMontaj.Trim();
        IntervalOrarPreferat = string.IsNullOrWhiteSpace(intervalOrarPreferat) ? null : intervalOrarPreferat.Trim();
        ObservatiiTransport = string.IsNullOrWhiteSpace(observatiiTransport) ? null : observatiiTransport.Trim();
    }

    public void SchimbaStatus(StatusProiect statusNou)
    {
        var eraFinalizat = Status == StatusProiect.Finalizat;

        Status = statusNou;

        if (statusNou == StatusProiect.Finalizat)
        {
            Progres = 100;
        }
        else if (eraFinalizat)
        {
            Progres = 0;
        }
    }

    public void ActualizeazaProgres(int progresNou)
    {
        if (progresNou is < 0 or > 100)
            throw new ArgumentOutOfRangeException(nameof(progresNou), "Progresul trebuie să fie între 0 și 100.");
        Progres = progresNou;
    }

    public void AdaugaNota(string nota)
    {
        Note = nota;
    }

    public void SeteazaUrmatorulPas(string? descriere, DateTime? termen)
    {
        DescriereUrmatorulPas = string.IsNullOrWhiteSpace(descriere) ? null : descriere.Trim();
        TermenUrmatorulPas = termen;
    }

    public void FinalizeazaUrmatorulPas()
    {
        DescriereUrmatorulPas = null;
        TermenUrmatorulPas = null;
    }
    
    public void ActualizeazaSetariOferta(decimal? avansProcent, int? termenExecutieZile, int? valabilitateOfertaZile)
    {
        if (avansProcent is < 0 or > 100)
            throw new ArgumentException("Avansul trebuie să fie între 0 și 100%.", nameof(avansProcent));
        if (termenExecutieZile is < 0)
            throw new ArgumentException("Termenul de execuție nu poate fi negativ.", nameof(termenExecutieZile));
        if (valabilitateOfertaZile is < 0)
            throw new ArgumentException("Valabilitatea ofertei nu poate fi negativă.", nameof(valabilitateOfertaZile));

        AvansProcent = avansProcent;
        TermenExecutieZile = termenExecutieZile;
        ValabilitateOfertaZile = valabilitateOfertaZile;
    }

}