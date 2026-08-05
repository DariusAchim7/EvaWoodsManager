namespace EvaWoods.Domain.Materiale;

public interface ICalculMaterialRepository
{
    Task AdaugaAsync(CalculMaterial calcul, CancellationToken ct = default);
    Task<IReadOnlyList<CalculMaterial>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task StergeAsync(CalculMaterial calcul, CancellationToken ct = default);
}