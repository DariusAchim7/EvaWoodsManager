using EvaWoods.Domain.Clienti;

namespace EvaWoods.Application.Clienti;

public record CreeazaClientRequest(
    string Nume,
    string Telefon,
    string Localitate,
    string? Email = null,
    string? Judet = null,
    StatusClient Status = StatusClient.Prospect);