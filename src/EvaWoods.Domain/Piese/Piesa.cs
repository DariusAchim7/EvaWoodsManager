namespace EvaWoods.Domain.Piese;

public enum DirectieFibra
{
    Fara,
    Verticala,
    Orizontala
}

public class Piesa
{
    public Guid Id { get; private set; }
    public Guid ProiectId { get; private set; }
    public Guid? CorpId { get; private set; }
    public string? Nume { get; private set; }
    public decimal LungimeMm { get; private set; }
    public decimal LatimeMm { get; private set; }
    public int Cantitate { get; private set; }
    public Guid? MaterialProdusId { get; private set; }
    public decimal? GrosimeMm { get; private set; }
    public int CantLaturiLungi { get; private set; }
    public int CantLaturiScurte { get; private set; }
    public Guid? MaterialCantProdusId { get; private set; }
    public DirectieFibra DirectieFibra { get; private set; }
    public bool PermiteRotire { get; private set; }
    public string? Observatii { get; private set; }
    public DateTime DataCreare { get; private set; }

    private Piesa() { }

    public static Piesa Creeaza(
        Guid proiectId,
        decimal lungimeMm,
        decimal latimeMm,
        int cantitate,
        string? nume = null,
        Guid? corpId = null,
        Guid? materialProdusId = null,
        decimal? grosimeMm = null,
        int cantLaturiLungi = 0,
        int cantLaturiScurte = 0,
        Guid? materialCantProdusId = null,
        DirectieFibra directieFibra = DirectieFibra.Fara,
        bool permiteRotire = false,
        string? observatii = null)
    {
        Valideaza(proiectId, lungimeMm, latimeMm, cantitate, cantLaturiLungi, cantLaturiScurte);

        return new Piesa
        {
            Id = Guid.NewGuid(),
            ProiectId = proiectId,
            CorpId = corpId,
            Nume = string.IsNullOrWhiteSpace(nume) ? null : nume.Trim(),
            LungimeMm = lungimeMm,
            LatimeMm = latimeMm,
            Cantitate = cantitate,
            MaterialProdusId = materialProdusId,
            GrosimeMm = grosimeMm,
            CantLaturiLungi = cantLaturiLungi,
            CantLaturiScurte = cantLaturiScurte,
            MaterialCantProdusId = materialCantProdusId,
            DirectieFibra = directieFibra,
            PermiteRotire = permiteRotire,
            Observatii = string.IsNullOrWhiteSpace(observatii) ? null : observatii.Trim(),
            DataCreare = DateTime.UtcNow
        };
    }

    public void Actualizeaza(
        decimal lungimeMm,
        decimal latimeMm,
        int cantitate,
        string? nume,
        Guid? corpId,
        Guid? materialProdusId,
        decimal? grosimeMm,
        int cantLaturiLungi,
        int cantLaturiScurte,
        Guid? materialCantProdusId,
        DirectieFibra directieFibra,
        bool permiteRotire,
        string? observatii)
    {
        Valideaza(ProiectId, lungimeMm, latimeMm, cantitate, cantLaturiLungi, cantLaturiScurte);

        LungimeMm = lungimeMm;
        LatimeMm = latimeMm;
        Cantitate = cantitate;
        Nume = string.IsNullOrWhiteSpace(nume) ? null : nume.Trim();
        CorpId = corpId;
        MaterialProdusId = materialProdusId;
        GrosimeMm = grosimeMm;
        CantLaturiLungi = cantLaturiLungi;
        CantLaturiScurte = cantLaturiScurte;
        MaterialCantProdusId = materialCantProdusId;
        DirectieFibra = directieFibra;
        PermiteRotire = permiteRotire;
        Observatii = string.IsNullOrWhiteSpace(observatii) ? null : observatii.Trim();
    }

    private static void Valideaza(Guid proiectId, decimal lungimeMm, decimal latimeMm, int cantitate, int cantLungi, int cantScurte)
    {
        if (proiectId == Guid.Empty)
            throw new ArgumentException("Proiectul este obligatoriu.");
        if (lungimeMm <= 0 || latimeMm <= 0)
            throw new ArgumentException("Dimensiunile trebuie să fie mai mari decât zero.");
        if (cantitate < 1)
            throw new ArgumentException("Cantitatea trebuie să fie cel puțin 1.");
        if (cantLungi is < 0 or > 2)
            throw new ArgumentException("Numărul de laturi lungi cantuite trebuie să fie între 0 și 2.");
        if (cantScurte is < 0 or > 2)
            throw new ArgumentException("Numărul de laturi scurte cantuite trebuie să fie între 0 și 2.");
    }

    public void AdaugaCantitate(int cantitateSuplimentara)
    {
        if (cantitateSuplimentara < 1)
            throw new ArgumentException("Cantitatea suplimentară trebuie să fie cel puțin 1.", nameof(cantitateSuplimentara));

        Cantitate += cantitateSuplimentara;
    }

    /// <summary>Total cant în ml, cu pierdere de 40mm pe fiecare latură cantuită.</summary>
    public decimal TotalCantMl =>
        (CantLaturiLungi * (LungimeMm + 40m) + CantLaturiScurte * (LatimeMm + 40m)) * Cantitate / 1000m;

    /// <summary>Suprafață totală în m².</summary>
    public decimal SuprafataM2 => LungimeMm * LatimeMm * Cantitate / 1_000_000m;
}