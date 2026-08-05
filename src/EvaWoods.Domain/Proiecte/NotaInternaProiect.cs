namespace EvaWoods.Domain.Proiecte;

public class NotaInternaProiect
{
    public Guid Id { get; private set; }
    public Guid ProiectId { get; private set; }
    public string Text { get; private set; } = string.Empty;
    public string? Autor { get; private set; }
    public bool EsteFixata { get; private set; }
    public bool EsteRezolvata { get; private set; }
    public DateTime DataCreare { get; private set; }
    public DateTime? DataModificare { get; private set; }

    private NotaInternaProiect() { }

    public static NotaInternaProiect Creeaza(Guid proiectId, string text, string? autor = null)
    {
        if (proiectId == Guid.Empty)
            throw new ArgumentException("Proiectul este obligatoriu.", nameof(proiectId));
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Textul notei este obligatoriu.", nameof(text));

        return new NotaInternaProiect
        {
            Id = Guid.NewGuid(),
            ProiectId = proiectId,
            Text = text.Trim(),
            Autor = string.IsNullOrWhiteSpace(autor) ? null : autor.Trim(),
            EsteFixata = false,
            EsteRezolvata = false,
            DataCreare = DateTime.UtcNow
        };
    }

    public void Actualizeaza(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Textul notei este obligatoriu.", nameof(text));

        Text = text.Trim();
        DataModificare = DateTime.UtcNow;
    }

    public void ComutaFixare() => EsteFixata = !EsteFixata;
    public void ComutaRezolvare() => EsteRezolvata = !EsteRezolvata;
}