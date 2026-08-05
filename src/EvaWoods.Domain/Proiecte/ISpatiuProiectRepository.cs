namespace EvaWoods.Domain.Proiecte;

public interface ISpatiuProiectRepository
{
    Task AdaugaAsync(SpatiuProiect spatiu, CancellationToken ct = default);
    Task<IReadOnlyList<SpatiuProiect>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task<SpatiuProiect?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default);
    Task StergeAsync(SpatiuProiect spatiu, CancellationToken ct = default);
}