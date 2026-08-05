using EvaWoods.Domain.Cheltuieli;

namespace EvaWoods.Application.Cheltuieli;

public record ActualizeazaCheltuialaRequest(
    Guid Id,
    string Descriere,
    CategorieCheltuiala Categorie,
    decimal ValoareFaraTva,
    decimal ProcentTva,
    DateTime Data,
    string? Subcategorie = null,
    Guid? FurnizorId = null,
    string? NumarFactura = null,
    MetodaPlata? MetodaPlata = null,
    bool IncludeInBuget = true,
    string? Observatii = null);