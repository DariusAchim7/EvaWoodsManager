namespace EvaWoods.Domain.Piese;

public interface ICorpProiectRepository
{
    Task AdaugaAsync(CorpProiect corp, CancellationToken ct = default);
    Task<IReadOnlyList<CorpProiect>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
}