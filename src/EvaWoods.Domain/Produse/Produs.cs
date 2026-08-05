namespace EvaWoods.Domain.Produse;

public class Produs
{
    public Guid Id { get; private set; }
    public string Cod { get; private set; } = string.Empty;
    public string Nume { get; private set; } = string.Empty;
    public CategorieProdus Categorie { get; private set; }
    public string? Subcategorie { get; private set; }
    public UnitateMasura UM { get; private set; }
    public decimal PretAchizitie { get; private set; }
    public decimal? PretCalcul { get; private set; }
    public decimal ProcentTva { get; private set; }
    public Guid? FurnizorId { get; private set; }
    public bool EsteActiv { get; private set; }
    public DateTime DataUltimeiActualizariPret { get; private set; }
    public string? Observatii { get; private set; }
    public DateTime DataCreare { get; private set; }
    public Guid? ServiciuAsociatId { get; private set; }

    // Doar pentru Categorie == Materiale, pentru algoritmul de debitare:
    public decimal? LungimeFoaieMm { get; private set; }
    public decimal? LatimeFoaieMm { get; private set; }
    public decimal? GrosimeMm { get; private set; }
    public bool PermiteRotire { get; private set; }
    public decimal? KerfMm { get; private set; }

    public decimal PretEfectivCalcul => PretCalcul ?? PretAchizitie;

    private Produs() { }

    public static Produs Creeaza(
        string cod,
        string nume,
        CategorieProdus categorie,
        UnitateMasura um,
        decimal pretAchizitie,
        decimal? pretCalcul = null,
        decimal procentTva = 19m,
        string? subcategorie = null,
        Guid? furnizorId = null,
        string? observatii = null,
        decimal? lungimeFoaieMm = null,
        decimal? latimeFoaieMm = null,
        decimal? grosimeMm = null,
        bool permiteRotire = false,
        decimal? kerfMm = null,
        Guid? serviciuAsociatId = null)
    {
        if (string.IsNullOrWhiteSpace(nume))
            throw new ArgumentException("Numele produsului este obligatoriu.", nameof(nume));
        if (pretAchizitie < 0)
            throw new ArgumentException("Prețul de achiziție nu poate fi negativ.", nameof(pretAchizitie));
        if (pretCalcul is < 0)
            throw new ArgumentException("Prețul de calcul nu poate fi negativ.", nameof(pretCalcul));

        if (categorie == CategorieProdus.Materiale)
        {
            if (lungimeFoaieMm is null or <= 0)
                throw new ArgumentException("Lungimea foii este obligatorie pentru materiale.", nameof(lungimeFoaieMm));
            if (latimeFoaieMm is null or <= 0)
                throw new ArgumentException("Lățimea foii este obligatorie pentru materiale.", nameof(latimeFoaieMm));
        }

        return new Produs
        {
            Id = Guid.NewGuid(),
            Cod = cod,
            Nume = nume.Trim(),
            Categorie = categorie,
            Subcategorie = string.IsNullOrWhiteSpace(subcategorie) ? null : subcategorie.Trim(),
            UM = um,
            PretAchizitie = pretAchizitie,
            PretCalcul = pretCalcul,
            ProcentTva = procentTva,
            FurnizorId = furnizorId,
            EsteActiv = true,
            DataUltimeiActualizariPret = DateTime.UtcNow,
            Observatii = string.IsNullOrWhiteSpace(observatii) ? null : observatii.Trim(),
            LungimeFoaieMm = categorie == CategorieProdus.Materiale ? lungimeFoaieMm : null,
            LatimeFoaieMm = categorie == CategorieProdus.Materiale ? latimeFoaieMm : null,
            GrosimeMm = categorie == CategorieProdus.Materiale ? grosimeMm : null,
            PermiteRotire = categorie == CategorieProdus.Materiale && permiteRotire,
            KerfMm = categorie == CategorieProdus.Materiale ? (kerfMm ?? 4m) : null,
            ServiciuAsociatId = serviciuAsociatId,
            DataCreare = DateTime.UtcNow
            
        };
    }

    public void Actualizeaza(
        string nume,
        CategorieProdus categorie,
        string? subcategorie,
        UnitateMasura um,
        decimal pretAchizitie,
        decimal? pretCalcul,
        decimal procentTva,
        Guid? furnizorId,
        string? observatii,
        decimal? lungimeFoaieMm,
        decimal? latimeFoaieMm,
        decimal? grosimeMm,
        bool permiteRotire,
        decimal? kerfMm,
        Guid? serviciuAsociatId)
    {
        if (string.IsNullOrWhiteSpace(nume))
            throw new ArgumentException("Numele produsului este obligatoriu.", nameof(nume));
        if (pretAchizitie < 0)
            throw new ArgumentException("Prețul de achiziție nu poate fi negativ.", nameof(pretAchizitie));

        var pretSchimbat = PretAchizitie != pretAchizitie || PretCalcul != pretCalcul;

        Nume = nume.Trim();
        Categorie = categorie;
        Subcategorie = string.IsNullOrWhiteSpace(subcategorie) ? null : subcategorie.Trim();
        UM = um;
        PretAchizitie = pretAchizitie;
        PretCalcul = pretCalcul;
        ProcentTva = procentTva;
        FurnizorId = furnizorId;
        Observatii = string.IsNullOrWhiteSpace(observatii) ? null : observatii.Trim();
        LungimeFoaieMm = categorie == CategorieProdus.Materiale ? lungimeFoaieMm : null;
        LatimeFoaieMm = categorie == CategorieProdus.Materiale ? latimeFoaieMm : null;
        GrosimeMm = categorie == CategorieProdus.Materiale ? grosimeMm : null;
        PermiteRotire = categorie == CategorieProdus.Materiale && permiteRotire;
        KerfMm = categorie == CategorieProdus.Materiale ? (kerfMm ?? 4m) : null;
        ServiciuAsociatId = serviciuAsociatId;


        if (pretSchimbat)
        {
            DataUltimeiActualizariPret = DateTime.UtcNow;
        }
    }

    public void ComutaActiv()
    {
        EsteActiv = !EsteActiv;
    }
}