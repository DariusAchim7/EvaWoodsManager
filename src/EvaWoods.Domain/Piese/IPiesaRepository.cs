namespace EvaWoods.Domain.Piese;

public interface IPiesaRepository
{
    Task AdaugaAsync(Piesa piesa, CancellationToken ct = default);
    Task<IReadOnlyList<Piesa>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task<Piesa?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default);
    Task StergeAsync(Piesa piesa, CancellationToken ct = default);
}