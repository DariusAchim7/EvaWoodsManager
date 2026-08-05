using EvaWoods.Domain.Piese;

namespace EvaWoods.Application.Piese;

public record PiesaDto(
    Guid Id,
    string? Nume,
    decimal LungimeMm,
    decimal LatimeMm,
    int Cantitate,
    Guid? CorpId,
    string? CorpNume,
    Guid? MaterialProdusId,
    string? MaterialNume,
    decimal? GrosimeMm,
    int CantLaturiLungi,
    int CantLaturiScurte,
    Guid? MaterialCantProdusId,
    string? MaterialCantNume,
    DirectieFibra DirectieFibra,
    bool PermiteRotire,
    string? Observatii,
    decimal TotalCantMl,
    decimal SuprafataM2);

public record CorpProiectDto(Guid Id, string Nume);