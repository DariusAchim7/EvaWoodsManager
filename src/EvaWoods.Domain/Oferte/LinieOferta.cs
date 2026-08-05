using EvaWoods.Domain.Produse;

namespace EvaWoods.Domain.Oferte;

public class LinieOferta
{
    public Guid Id { get; private set; }
    public Guid ProiectId { get; private set; }
    public Guid? ProdusId { get; private set; }
    public string DescriereSnapshot { get; private set; } = string.Empty;
    public CategorieProdus CategorieSnapshot { get; private set; }
    public UnitateMasura UMSnapshot { get; private set; }
    public decimal CantitateCalculata { get; private set; }
    public decimal CantitateOfertata { get; private set; }
    public decimal PretCostSnapshot { get; private set; }
    public decimal PretVanzareSnapshot { get; private set; }
    public decimal ProcentTva { get; private set; }
    public string? Nota { get; private set; }
    public bool VizibilClient { get; private set; }
    public bool EsteAutomat { get; private set; }
    public int Ordine { get; private set; }
    public DateTime DataAdaugare { get; private set; }

    public decimal Total => CantitateOfertata * PretVanzareSnapshot;
    public decimal TotalCuTva => Total * (1 + ProcentTva / 100m);

    private LinieOferta() { }

    public static LinieOferta Creeaza(
        Guid proiectId,
        Guid? produsId,
        string descriere,
        CategorieProdus categorie,
        UnitateMasura um,
        decimal cantitateCalculata,
        decimal cantitateOfertata,
        decimal pretCost,
        decimal pretVanzare,
        bool esteAutomat,
        int ordine,
        decimal procentTva = 19m,
        bool vizibilClient = true,
        string? nota = null)
    {
        if (proiectId == Guid.Empty)
            throw new ArgumentException("Proiectul este obligatoriu.", nameof(proiectId));
        if (string.IsNullOrWhiteSpace(descriere))
            throw new ArgumentException("Descrierea este obligatorie.", nameof(descriere));
        if (cantitateOfertata < 0)
            throw new ArgumentException("Cantitatea nu poate fi negativă.", nameof(cantitateOfertata));

        return new LinieOferta
        {
            Id = Guid.NewGuid(),
            ProiectId = proiectId,
            ProdusId = produsId,
            DescriereSnapshot = descriere.Trim(),
            CategorieSnapshot = categorie,
            UMSnapshot = um,
            CantitateCalculata = cantitateCalculata,
            CantitateOfertata = cantitateOfertata,
            PretCostSnapshot = pretCost,
            PretVanzareSnapshot = pretVanzare,
            ProcentTva = procentTva,
            Nota = string.IsNullOrWhiteSpace(nota) ? null : nota.Trim(),
            VizibilClient = vizibilClient,
            EsteAutomat = esteAutomat,
            Ordine = ordine,
            DataAdaugare = DateTime.UtcNow
        };
    }

    public void ActualizeazaCantitate(decimal cantitateOfertata)
    {
        if (EsteAutomat)
            throw new InvalidOperationException("Rândurile automate (Materiale/Cant) se editează din tab-ul Materiale.");
        if (cantitateOfertata < 0)
            throw new ArgumentException("Cantitatea nu poate fi negativă.", nameof(cantitateOfertata));

        CantitateCalculata = cantitateOfertata;
        CantitateOfertata = cantitateOfertata;
    }

    public void ComutaVizibilClient()
    {
        VizibilClient = !VizibilClient;
    }
}