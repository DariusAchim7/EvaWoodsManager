namespace EvaWoods.Application.Proiecte;

public enum Severitate
{
    Info,
    Atentionare,
    Critica
}

public record AlertaDto(string Mesaj, Severitate Severitate);