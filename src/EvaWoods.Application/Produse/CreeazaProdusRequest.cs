using EvaWoods.Domain.Produse;

namespace EvaWoods.Application.Produse;

public record CreeazaProdusRequest(
    string Nume,
    CategorieProdus Categorie,
    UnitateMasura UM,
    decimal PretAchizitie,
    decimal? PretCalcul = null,
    decimal ProcentTva = 19m,
    string? Subcategorie = null,
    Guid? FurnizorId = null,
    string? Observatii = null,
    decimal? LungimeFoaieMm = null,
    decimal? LatimeFoaieMm = null,
    decimal? GrosimeMm = null,
    bool PermiteRotire = false,
    decimal? KerfMm = null,
    Guid? ServiciuAsociatId = null);