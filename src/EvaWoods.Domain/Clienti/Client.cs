namespace EvaWoods.Domain.Clienti;

public enum StatusClient
{
    Prospect,
    Activ,
    Inactiv
}

public class Client
{
    public Guid Id { get; private set; }
    public string Nume { get; private set; } = string.Empty;
    public string Telefon { get; private set; } = string.Empty;
    public string? Email { get; private set; }
    public string Localitate { get; private set; } = string.Empty;
    public string? Judet { get; private set; }
    public StatusClient Status { get; private set; }
    public string? Note { get; private set; }
    public DateTime DataCreare { get; private set; }
    public DateTime? DataUltimeiActivitati { get; private set; }

    private Client() { } // necesar pentru EF Core

    public static Client Creeaza(
        string nume,
        string telefon,
        string localitate,
        string? email = null,
        string? judet = null,
        StatusClient status = StatusClient.Prospect)
    {
        if (string.IsNullOrWhiteSpace(nume))
            throw new ArgumentException("Numele clientului este obligatoriu.", nameof(nume));
        if (string.IsNullOrWhiteSpace(telefon))
            throw new ArgumentException("Telefonul este obligatoriu.", nameof(telefon));
        if (string.IsNullOrWhiteSpace(localitate))
            throw new ArgumentException("Localitatea este obligatorie.", nameof(localitate));

        return new Client
        {
            Id = Guid.NewGuid(),
            Nume = nume.Trim(),
            Telefon = telefon.Trim(),
            Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim(),
            Localitate = localitate.Trim(),
            Judet = string.IsNullOrWhiteSpace(judet) ? null : judet.Trim(),
            Status = status,
            DataCreare = DateTime.UtcNow
        };
    }

    public void ActualizeazaDateContact(string telefon, string? email)
    {
        if (string.IsNullOrWhiteSpace(telefon))
            throw new ArgumentException("Telefonul este obligatoriu.", nameof(telefon));

        Telefon = telefon.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
    }

    public void Actualizeaza(
        string nume,
        string telefon,
        string localitate,
        string? email,
        string? judet,
        StatusClient status)
    {
        if (string.IsNullOrWhiteSpace(nume))
            throw new ArgumentException("Numele clientului este obligatoriu.", nameof(nume));
        if (string.IsNullOrWhiteSpace(telefon))
            throw new ArgumentException("Telefonul este obligatoriu.", nameof(telefon));
        if (string.IsNullOrWhiteSpace(localitate))
            throw new ArgumentException("Localitatea este obligatorie.", nameof(localitate));

        Nume = nume.Trim();
        Telefon = telefon.Trim();
        Localitate = localitate.Trim();
        Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim();
        Judet = string.IsNullOrWhiteSpace(judet) ? null : judet.Trim();
        Status = status;
    }

    public void SchimbaStatus(StatusClient statusNou)
    {
        Status = statusNou;
    }

    public void AdaugaNota(string nota)
    {
        Note = nota;
        DataUltimeiActivitati = DateTime.UtcNow;
    }
}