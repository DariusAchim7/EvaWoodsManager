namespace EvaWoods.Domain.Proiecte;

public interface IProiectRepository
{
    Task AdaugaAsync(Proiect proiect, CancellationToken ct = default);
    Task<Proiect?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Proiect>> ObtineToateAsync(CancellationToken ct = default);
    Task<int> ObtineNumarTotalAsync(CancellationToken ct = default);
    Task<IReadOnlyList<Proiect>> ObtineDupaClientAsync(Guid clientId, CancellationToken ct = default);
}