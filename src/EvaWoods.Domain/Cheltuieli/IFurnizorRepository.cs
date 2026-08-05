namespace EvaWoods.Domain.Cheltuieli;

public interface IFurnizorRepository
{
    Task AdaugaAsync(Furnizor furnizor, CancellationToken ct = default);
    Task<IReadOnlyList<Furnizor>> ObtineToateAsync(CancellationToken ct = default);
}