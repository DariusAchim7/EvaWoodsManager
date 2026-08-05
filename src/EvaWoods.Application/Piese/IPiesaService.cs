namespace EvaWoods.Application.Piese;

public interface IPiesaService
{
    Task AdaugaPieseAsync(IReadOnlyList<CreeazaPiesaRequest> requests, CancellationToken ct = default);
    Task<IReadOnlyList<PiesaDto>> ObtinePieseDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task ActualizeazaPiesaAsync(ActualizeazaPiesaRequest request, CancellationToken ct = default);
    Task<Guid> DuplicaPiesaAsync(Guid piesaId, CancellationToken ct = default);
    Task StergePiesaAsync(Guid piesaId, CancellationToken ct = default);
    Task<Guid> AdaugaCorpAsync(Guid proiectId, string nume, CancellationToken ct = default);
    Task<IReadOnlyList<CorpProiectDto>> ObtineCorpuriDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task<int> CombinaPieseleIdenticeAsync(Guid proiectId, CancellationToken ct = default);
}