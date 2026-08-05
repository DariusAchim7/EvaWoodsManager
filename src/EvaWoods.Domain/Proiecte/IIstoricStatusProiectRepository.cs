namespace EvaWoods.Domain.Proiecte;

public interface IIstoricStatusProiectRepository
{
    Task AdaugaAsync(IstoricStatusProiect intrare, CancellationToken ct = default);
    Task<IReadOnlyList<IstoricStatusProiect>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
}