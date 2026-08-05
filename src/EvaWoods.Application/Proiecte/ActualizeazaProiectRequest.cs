using EvaWoods.Domain.Proiecte;

namespace EvaWoods.Application.Proiecte;

public record ActualizeazaProiectRequest(
    Guid Id,
    string Nume,
    decimal Valoare,
    string? Subtip,
    TipPrincipalProiect? TipPrincipal,
    string? Descriere,
    DateTime? TermenLimita,
    string? AdresaMontaj,
    decimal? BugetAlocat,
    decimal? ProcentManopera);