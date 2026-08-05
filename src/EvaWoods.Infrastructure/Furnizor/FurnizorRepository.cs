using EvaWoods.Domain.Cheltuieli;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Cheltuieli;

public class FurnizorRepository : IFurnizorRepository
{
    private readonly ApplicationDbContext _context;

    public FurnizorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(Furnizor furnizor, CancellationToken ct = default)
    {
        await _context.Furnizori.AddAsync(furnizor, ct);
    }

    public async Task<IReadOnlyList<Furnizor>> ObtineToateAsync(CancellationToken ct = default)
    {
        return await _context.Furnizori.OrderBy(f => f.Nume).ToListAsync(ct);
    }
}