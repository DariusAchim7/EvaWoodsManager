namespace EvaWoods.Application.Proiecte;

public record SpatiuProiectDto(
    Guid Id,
    string Nume,
    decimal? Suprafata,
    int? InaltimeMm,
    string? Orientare,
    string? TipSpatiu,
    string? StarePereti,
    string? StarePardoseala);