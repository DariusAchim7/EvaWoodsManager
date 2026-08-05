using EvaWoods.Domain.Cheltuieli;

namespace EvaWoods.Application.Cheltuieli;

public record CreeazaCheltuialaRequest(
    Guid ProiectId,
    string Descriere,
    CategorieCheltuiala Categorie,
    decimal ValoareFaraTva,
    decimal ProcentTva,
    DateTime Data,
    StatusCheltuiala Status,
    string? Subcategorie = null,
    Guid? FurnizorId = null,
    string? NumarFactura = null,
    MetodaPlata? MetodaPlata = null,
    StatusPlata StatusPlata = StatusPlata.InAsteptare,
    bool IncludeInBuget = true,
    string? Observatii = null);