namespace EvaWoods.Domain.Proiecte;

public class IstoricStatusProiect
{
    public Guid Id { get; private set; }
    public Guid ProiectId { get; private set; }
    public StatusProiect StatusVechi { get; private set; }
    public StatusProiect StatusNou { get; private set; }
    public string? Observatie { get; private set; }
    public string? Utilizator { get; private set; }
    public DateTime DataSchimbare { get; private set; }

    private IstoricStatusProiect() { }

    public static IstoricStatusProiect Creeaza(
        Guid proiectId,
        StatusProiect statusVechi,
        StatusProiect statusNou,
        string? observatie = null,
        string? utilizator = null)
    {
        return new IstoricStatusProiect
        {
            Id = Guid.NewGuid(),
            ProiectId = proiectId,
            StatusVechi = statusVechi,
            StatusNou = statusNou,
            Observatie = string.IsNullOrWhiteSpace(observatie) ? null : observatie.Trim(),
            Utilizator = string.IsNullOrWhiteSpace(utilizator) ? null : utilizator.Trim(),
            DataSchimbare = DateTime.UtcNow
        };
    }
}