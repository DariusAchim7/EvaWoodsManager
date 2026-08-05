using EvaWoods.Domain.Produse;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Produse;

public class ProdusRepository : IProdusRepository
{
    private readonly ApplicationDbContext _context;

    public ProdusRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(Produs produs, CancellationToken ct = default)
    {
        await _context.Produse.AddAsync(produs, ct);
    }

    public async Task<IReadOnlyList<Produs>> ObtineToateAsync(CancellationToken ct = default)
    {
        return await _context.Produse.OrderBy(p => p.Nume).ToListAsync(ct);
    }

    public async Task<Produs?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Produse.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task StergeAsync(Produs produs, CancellationToken ct = default)
    {
        _context.Produse.Remove(produs);
        await Task.CompletedTask;
    }

    public async Task<int> ObtineNumarPeCategorieAsync(CategorieProdus categorie, CancellationToken ct = default)
    {
        return await _context.Produse.CountAsync(p => p.Categorie == categorie, ct);
    }
}