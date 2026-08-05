using EvaWoods.Domain.Proiecte;

namespace EvaWoods.Application.Proiecte;

public record IstoricStatusDto(
    Guid Id,
    StatusProiect StatusVechi,
    StatusProiect StatusNou,
    string? Observatie,
    string? Utilizator,
    DateTime DataSchimbare);