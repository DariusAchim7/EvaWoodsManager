using EvaWoods.Domain.Materiale;

namespace EvaWoods.Application.Materiale;

public record CalculMaterialDto(
    Guid Id,
    Guid MaterialProdusId,
    string MaterialNume,
    string? MaterialSubcategorie,
    decimal? GrosimeMm,
    decimal? LungimeFoaieMm,
    decimal? LatimeFoaieMm,
    int NumarTipuriPiese,
    int NumarBucati,
    decimal SuprafataPieseM2,
    decimal FoiTeoretice,
    int FoiEstimate,
    decimal UtilizareProcent,
    decimal PierdereProcent,
    string? LayoutJson,
    string? Erori,
    StatusCalculMaterial Status,
    DateTime DataCalcul,
    decimal? PretFoaie);