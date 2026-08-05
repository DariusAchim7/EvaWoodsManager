namespace EvaWoods.Domain.Cheltuieli;

public enum CategorieCheltuiala
{
    Materiale,
    Servicii,
    AlteCheltuieli
}

public enum MetodaPlata
{
    Numerar,
    Card,
    TransferBancar,
    Altul
}

public enum StatusPlata
{
    Platita,
    InAsteptare
}

public enum StatusCheltuiala
{
    Ciorna,
    Confirmata
}

public class Cheltuiala
{
    public Guid Id { get; private set; }
    public Guid ProiectId { get; private set; }
    public string Descriere { get; private set; } = string.Empty;
    public CategorieCheltuiala Categorie { get; private set; }
    public string? Subcategorie { get; private set; }
    public Guid? FurnizorId { get; private set; }
    public DateTime Data { get; private set; }
    public string? NumarFactura { get; private set; }
    public decimal ValoareFaraTva { get; private set; }
    public decimal ProcentTva { get; private set; }
    public decimal Tva { get; private set; }
    public decimal Total { get; private set; }
    public MetodaPlata? MetodaPlata { get; private set; }
    public StatusPlata StatusPlata { get; private set; }
    public StatusCheltuiala Status { get; private set; }
    public bool IncludeInBuget { get; private set; }
    public string? Observatii { get; private set; }
    public DateTime DataCreare { get; private set; }

    private Cheltuiala() { }

    public static Cheltuiala Creeaza(
        Guid proiectId,
        string descriere,
        CategorieCheltuiala categorie,
        decimal valoareFaraTva,
        decimal procentTva,
        DateTime data,
        StatusCheltuiala status,
        string? subcategorie = null,
        Guid? furnizorId = null,
        string? numarFactura = null,
        MetodaPlata? metodaPlata = null,
        StatusPlata statusPlata = StatusPlata.InAsteptare,
        bool includeInBuget = true,
        string? observatii = null)
    {
        if (proiectId == Guid.Empty)
            throw new ArgumentException("Proiectul este obligatoriu.", nameof(proiectId));
        if (string.IsNullOrWhiteSpace(descriere))
            throw new ArgumentException("Descrierea este obligatorie.", nameof(descriere));
        if (valoareFaraTva < 0)
            throw new ArgumentException("Valoarea nu poate fi negativă.", nameof(valoareFaraTva));
        if (procentTva < 0)
            throw new ArgumentException("Procentul de TVA nu poate fi negativ.", nameof(procentTva));

        var tva = Math.Round(valoareFaraTva * procentTva / 100m, 2);
        var total = valoareFaraTva + tva;

        return new Cheltuiala
        {
            Id = Guid.NewGuid(),
            ProiectId = proiectId,
            Descriere = descriere.Trim(),
            Categorie = categorie,
            Subcategorie = string.IsNullOrWhiteSpace(subcategorie) ? null : subcategorie.Trim(),
            FurnizorId = furnizorId,
            Data = data,
            NumarFactura = string.IsNullOrWhiteSpace(numarFactura) ? null : numarFactura.Trim(),
            ValoareFaraTva = valoareFaraTva,
            ProcentTva = procentTva,
            Tva = tva,
            Total = total,
            MetodaPlata = metodaPlata,
            StatusPlata = statusPlata,
            Status = status,
            IncludeInBuget = includeInBuget,
            Observatii = string.IsNullOrWhiteSpace(observatii) ? null : observatii.Trim(),
            DataCreare = DateTime.UtcNow
        };
    }

    public void Actualizeaza(
        string descriere,
        CategorieCheltuiala categorie,
        decimal valoareFaraTva,
        decimal procentTva,
        DateTime data,
        string? subcategorie,
        Guid? furnizorId,
        string? numarFactura,
        MetodaPlata? metodaPlata,
        bool includeInBuget,
        string? observatii)
    {
        if (string.IsNullOrWhiteSpace(descriere))
            throw new ArgumentException("Descrierea este obligatorie.", nameof(descriere));
        if (valoareFaraTva < 0)
            throw new ArgumentException("Valoarea nu poate fi negativă.", nameof(valoareFaraTva));
        if (procentTva < 0)
            throw new ArgumentException("Procentul de TVA nu poate fi negativ.", nameof(procentTva));

        Descriere = descriere.Trim();
        Categorie = categorie;
        Subcategorie = string.IsNullOrWhiteSpace(subcategorie) ? null : subcategorie.Trim();
        FurnizorId = furnizorId;
        Data = data;
        NumarFactura = string.IsNullOrWhiteSpace(numarFactura) ? null : numarFactura.Trim();
        ValoareFaraTva = valoareFaraTva;
        ProcentTva = procentTva;
        Tva = Math.Round(valoareFaraTva * procentTva / 100m, 2);
        Total = ValoareFaraTva + Tva;
        MetodaPlata = metodaPlata;
        IncludeInBuget = includeInBuget;
        Observatii = string.IsNullOrWhiteSpace(observatii) ? null : observatii.Trim();
    }

    public void MarcheazaCaPlatita()
    {
        StatusPlata = StatusPlata.Platita;
    }
}