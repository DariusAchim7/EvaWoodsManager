namespace EvaWoods.Domain.Materiale;

public enum StatusCalculMaterial
{
    Neprocesat,
    CalculatPeSuprafata,
    Optimizat,
    NecesitaRecalculare,
    CuErori
}

public class CalculMaterial
{
    public Guid Id { get; private set; }
    public Guid ProiectId { get; private set; }
    public Guid MaterialProdusId { get; private set; }
    public int NumarTipuriPiese { get; private set; }
    public int NumarBucati { get; private set; }
    public decimal SuprafataPieseM2 { get; private set; }
    public decimal FoiTeoretice { get; private set; }
    public int FoiEstimate { get; private set; }
    public decimal UtilizareProcent { get; private set; }
    public decimal PierdereProcent { get; private set; }
    public string? LayoutJson { get; private set; }
    public string? Erori { get; private set; }
    public StatusCalculMaterial Status { get; private set; }
    public DateTime DataCalcul { get; private set; }

    private CalculMaterial() { }

    public static CalculMaterial Creeaza(Guid proiectId, Guid materialProdusId)
    {
        if (proiectId == Guid.Empty)
            throw new ArgumentException("Proiectul este obligatoriu.", nameof(proiectId));
        if (materialProdusId == Guid.Empty)
            throw new ArgumentException("Materialul este obligatoriu.", nameof(materialProdusId));

        return new CalculMaterial
        {
            Id = Guid.NewGuid(),
            ProiectId = proiectId,
            MaterialProdusId = materialProdusId,
            Status = StatusCalculMaterial.Neprocesat,
            DataCalcul = DateTime.UtcNow
        };
    }

    public void ActualizeazaRezultat(
        int numarTipuriPiese,
        int numarBucati,
        decimal suprafataPieseM2,
        decimal foiTeoretice,
        int foiEstimate,
        decimal utilizareProcent,
        string? layoutJson,
        string? erori)
    {
        NumarTipuriPiese = numarTipuriPiese;
        NumarBucati = numarBucati;
        SuprafataPieseM2 = suprafataPieseM2;
        FoiTeoretice = foiTeoretice;
        FoiEstimate = foiEstimate;
        UtilizareProcent = utilizareProcent;
        PierdereProcent = utilizareProcent > 0 ? 100m - utilizareProcent : 0m;
        LayoutJson = layoutJson;
        Erori = erori;
        Status = string.IsNullOrWhiteSpace(erori) ? StatusCalculMaterial.Optimizat : StatusCalculMaterial.CuErori;
        DataCalcul = DateTime.UtcNow;
    }

    public void MarcheazaNecesitaRecalculare()
    {
        if (Status is StatusCalculMaterial.Optimizat or StatusCalculMaterial.CalculatPeSuprafata)
        {
            Status = StatusCalculMaterial.NecesitaRecalculare;
        }
    }
}