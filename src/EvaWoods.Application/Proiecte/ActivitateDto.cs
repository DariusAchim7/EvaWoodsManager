namespace EvaWoods.Application.Proiecte;

public enum TipActivitate
{
    SchimbareStatus,
    NotaAdaugata,
    NotaModificata
}

public record ActivitateDto(
    TipActivitate Tip,
    string Descriere,
    DateTime Data,
    string? Utilizator);