using EvaWoods.Domain.Cheltuieli;

namespace EvaWoods.Application.Cheltuieli;

public interface ICheltuialaService
{
    Task<Guid> CreeazaCheltuialaAsync(CreeazaCheltuialaRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<CheltuialaDto>> ObtineCheltuieliDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task<CheltuialaDto?> ObtineCheltuialaDupaIdAsync(Guid id, CancellationToken ct = default);
    Task ActualizeazaCheltuialaAsync(ActualizeazaCheltuialaRequest request, CancellationToken ct = default);
    Task MarcheazaCaPlatitaAsync(Guid cheltuialaId, CancellationToken ct = default);
    Task<Guid> DuplicaCheltuialaAsync(Guid cheltuialaId, CancellationToken ct = default);
    Task StergeCheltuialaAsync(Guid cheltuialaId, CancellationToken ct = default);
    Task<Guid> AdaugaFurnizorAsync(string nume, CancellationToken ct = default);
    Task<IReadOnlyList<FurnizorDto>> ObtineFurnizoriAsync(CancellationToken ct = default);
    Task<decimal?> ObtineBugetRamasAsync(Guid proiectId, CancellationToken ct = default);
}