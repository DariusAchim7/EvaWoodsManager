using EvaWoods.Domain.Piese;

namespace EvaWoods.Application.Piese;

public record ActualizeazaPiesaRequest(
    Guid Id,
    decimal LungimeMm,
    decimal LatimeMm,
    int Cantitate,
    string? Nume,
    Guid? CorpId,
    Guid? MaterialProdusId,
    decimal? GrosimeMm,
    int CantLaturiLungi,
    int CantLaturiScurte,
    Guid? MaterialCantProdusId,
    DirectieFibra DirectieFibra,
    bool PermiteRotire,
    string? Observatii);