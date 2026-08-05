using EvaWoods.Domain.Produse;

namespace EvaWoods.Application.Produse;

public record ActualizeazaProdusRequest(
    Guid Id,
    string Nume,
    CategorieProdus Categorie,
    string? Subcategorie,
    UnitateMasura UM,
    decimal PretAchizitie,
    decimal? PretCalcul,
    decimal ProcentTva,
    Guid? FurnizorId,
    string? Observatii,
    decimal? LungimeFoaieMm,
    decimal? LatimeFoaieMm,
    decimal? GrosimeMm,
    bool PermiteRotire,
    decimal? KerfMm,
    Guid? ServiciuAsociatId);