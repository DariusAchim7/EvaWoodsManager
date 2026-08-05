namespace EvaWoods.Application.Materiale;

public interface ICalculMaterialService
{
    Task RecalculeazaAsync(Guid proiectId, CancellationToken ct = default);
    Task<IReadOnlyList<CalculMaterialDto>> ObtineCalculeAsync(Guid proiectId, CancellationToken ct = default);
}