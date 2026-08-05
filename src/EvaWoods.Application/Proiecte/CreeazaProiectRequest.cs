using EvaWoods.Domain.Proiecte;

namespace EvaWoods.Application.Proiecte;

public record CreeazaProiectRequest(
    Guid ClientId,
    string Nume,
    decimal Valoare,
    string? Subtip = null,
    TipPrincipalProiect? TipPrincipal = null,
    string? Descriere = null,
    DateTime? TermenLimita = null,
    StatusProiect Status = StatusProiect.OfertaTrimisa,
    string? AdresaMontaj = null);