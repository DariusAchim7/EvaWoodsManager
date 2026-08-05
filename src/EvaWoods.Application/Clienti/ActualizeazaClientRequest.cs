using EvaWoods.Domain.Clienti;

namespace EvaWoods.Application.Clienti;

public record ActualizeazaClientRequest(
    Guid Id,
    string Nume,
    string Telefon,
    string Localitate,
    string? Email,
    string? Judet,
    StatusClient Status);