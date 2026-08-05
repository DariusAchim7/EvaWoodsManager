namespace EvaWoods.Application.Proiecte;

public record AdaugaSpatiuRequest(
    Guid ProiectId,
    string Nume,
    decimal? Suprafata = null,
    int? InaltimeMm = null,
    string? Orientare = null,
    string? TipSpatiu = null,
    string? StarePereti = null,
    string? StarePardoseala = null);