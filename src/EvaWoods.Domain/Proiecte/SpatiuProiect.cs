namespace EvaWoods.Domain.Proiecte;

public class SpatiuProiect
{
    public Guid Id { get; private set; }
    public Guid ProiectId { get; private set; }
    public string Nume { get; private set; } = string.Empty;
    public decimal? Suprafata { get; private set; }
    public int? InaltimeMm { get; private set; }
    public string? Orientare { get; private set; }
    public string? TipSpatiu { get; private set; }
    public string? StarePereti { get; private set; }
    public string? StarePardoseala { get; private set; }
    public DateTime DataCreare { get; private set; }

    private SpatiuProiect() { }

    public static SpatiuProiect Creeaza(
        Guid proiectId,
        string nume,
        decimal? suprafata = null,
        int? inaltimeMm = null,
        string? orientare = null,
        string? tipSpatiu = null,
        string? starePereti = null,
        string? starePardoseala = null)
    {
        if (proiectId == Guid.Empty)
            throw new ArgumentException("Proiectul este obligatoriu.", nameof(proiectId));
        if (string.IsNullOrWhiteSpace(nume))
            throw new ArgumentException("Numele camerei/spațiului este obligatoriu.", nameof(nume));

        return new SpatiuProiect
        {
            Id = Guid.NewGuid(),
            ProiectId = proiectId,
            Nume = nume.Trim(),
            Suprafata = suprafata,
            InaltimeMm = inaltimeMm,
            Orientare = string.IsNullOrWhiteSpace(orientare) ? null : orientare.Trim(),
            TipSpatiu = string.IsNullOrWhiteSpace(tipSpatiu) ? null : tipSpatiu.Trim(),
            StarePereti = string.IsNullOrWhiteSpace(starePereti) ? null : starePereti.Trim(),
            StarePardoseala = string.IsNullOrWhiteSpace(starePardoseala) ? null : starePardoseala.Trim(),
            DataCreare = DateTime.UtcNow
        };
    }
}