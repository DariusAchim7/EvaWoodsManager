using EvaWoods.Domain.Produse;

namespace EvaWoods.Application.Produse;

public record ProdusDto(
    Guid Id,
    string Cod,
    string Nume,
    CategorieProdus Categorie,
    string? Subcategorie,
    UnitateMasura UM,
    decimal PretAchizitie,
    decimal? PretCalcul,
    decimal ProcentTva,
    Guid? FurnizorId,
    string? FurnizorNume,
    bool EsteActiv,
    DateTime DataUltimeiActualizariPret,
    string? Observatii,
    decimal? LungimeFoaieMm,
    decimal? LatimeFoaieMm,
    decimal? GrosimeMm,
    bool PermiteRotire,
    decimal? KerfMm,
    Guid? ServiciuAsociatId,
    string? ServiciuAsociatNume);