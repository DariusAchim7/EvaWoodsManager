namespace EvaWoods.Application.Proiecte;

public record NotaInternaDto(
    Guid Id,
    string Text,
    string? Autor,
    bool EsteFixata,
    bool EsteRezolvata,
    DateTime DataCreare,
    DateTime? DataModificare);