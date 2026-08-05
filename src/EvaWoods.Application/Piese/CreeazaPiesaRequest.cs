using EvaWoods.Domain.Piese;

namespace EvaWoods.Application.Piese;

public record CreeazaPiesaRequest(
    Guid ProiectId,
    decimal LungimeMm,
    decimal LatimeMm,
    int Cantitate,
    string? Nume = null,
    Guid? CorpId = null,
    Guid? MaterialProdusId = null,
    decimal? GrosimeMm = null,
    int CantLaturiLungi = 0,
    int CantLaturiScurte = 0,
    Guid? MaterialCantProdusId = null,
    DirectieFibra DirectieFibra = DirectieFibra.Fara,
    bool PermiteRotire = false,
    string? Observatii = null);