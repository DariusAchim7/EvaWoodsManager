namespace EvaWoods.Application.Proiecte;

public record AdaugaNotaInternaRequest(Guid ProiectId, string Text, string? Autor = null);