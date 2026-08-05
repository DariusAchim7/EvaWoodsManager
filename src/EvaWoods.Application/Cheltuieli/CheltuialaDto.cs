using EvaWoods.Domain.Cheltuieli;

namespace EvaWoods.Application.Cheltuieli;

public record CheltuialaDto(
    Guid Id,
    string Descriere,
    CategorieCheltuiala Categorie,
    string? Subcategorie,
    Guid? FurnizorId,
    string? FurnizorNume,
    DateTime Data,
    string? NumarFactura,
    decimal ValoareFaraTva,
    decimal ProcentTva,
    decimal Tva,
    decimal Total,
    MetodaPlata? MetodaPlata,
    StatusPlata StatusPlata,
    StatusCheltuiala Status,
    bool IncludeInBuget,
    string? Observatii);