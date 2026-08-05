namespace EvaWoods.Domain.Produse;

public interface IProdusRepository
{
    Task AdaugaAsync(Produs produs, CancellationToken ct = default);
    Task<IReadOnlyList<Produs>> ObtineToateAsync(CancellationToken ct = default);
    Task<Produs?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default);
    Task StergeAsync(Produs produs, CancellationToken ct = default);
    Task<int> ObtineNumarPeCategorieAsync(CategorieProdus categorie, CancellationToken ct = default);
}