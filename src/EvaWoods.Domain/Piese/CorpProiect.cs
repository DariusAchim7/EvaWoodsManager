namespace EvaWoods.Domain.Piese;

public class CorpProiect
{
    public Guid Id { get; private set; }
    public Guid ProiectId { get; private set; }
    public string Nume { get; private set; } = string.Empty;
    public DateTime DataCreare { get; private set; }

    private CorpProiect() { }

    public static CorpProiect Creeaza(Guid proiectId, string nume)
    {
        if (proiectId == Guid.Empty)
            throw new ArgumentException("Proiectul este obligatoriu.", nameof(proiectId));
        if (string.IsNullOrWhiteSpace(nume))
            throw new ArgumentException("Numele corpului este obligatoriu.", nameof(nume));

        return new CorpProiect
        {
            Id = Guid.NewGuid(),
            ProiectId = proiectId,
            Nume = nume.Trim(),
            DataCreare = DateTime.UtcNow
        };
    }
}