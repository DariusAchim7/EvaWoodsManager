using EvaWoods.Domain.Produse;

namespace EvaWoods.Application.Produse;

public interface IProdusService
{
    Task<Guid> CreeazaProdusAsync(CreeazaProdusRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<ProdusDto>> ObtineToateProduseleAsync(CancellationToken ct = default);
    Task ActualizeazaProdusAsync(ActualizeazaProdusRequest request, CancellationToken ct = default);
    Task ComutaActivProdusAsync(Guid id, CancellationToken ct = default);
    Task StergeProdusAsync(Guid id, CancellationToken ct = default);
}