namespace EvaWoods.Domain.Cheltuieli;

public interface ICheltuialaRepository
{
    Task AdaugaAsync(Cheltuiala cheltuiala, CancellationToken ct = default);
    Task<IReadOnlyList<Cheltuiala>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task<Cheltuiala?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default);
    Task StergeAsync(Cheltuiala cheltuiala, CancellationToken ct = default);
}