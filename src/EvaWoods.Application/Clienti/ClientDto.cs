using EvaWoods.Domain.Clienti;

namespace EvaWoods.Application.Clienti;

public record ClientDto(
    Guid Id,
    string Nume,
    string Telefon,
    string? Email,
    string Localitate,
    string? Judet,
    StatusClient Status,
    DateTime? DataUltimeiActivitati,
    int NumarProiecte,
    string? Note);