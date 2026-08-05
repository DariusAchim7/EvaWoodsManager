using EvaWoods.Domain.Proiecte;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Proiecte;

public class ProiectRepository : IProiectRepository
{
    private readonly ApplicationDbContext _context;

    public ProiectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(Proiect proiect, CancellationToken ct = default)
    {
        await _context.Proiecte.AddAsync(proiect, ct);
    }

    public async Task<Proiect?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Proiecte.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task<IReadOnlyList<Proiect>> ObtineToateAsync(CancellationToken ct = default)
    {
        return await _context.Proiecte
            .OrderBy(p => p.TermenLimita)
            .ToListAsync(ct);
    }

    public async Task<int> ObtineNumarTotalAsync(CancellationToken ct = default)
    {
        return await _context.Proiecte.CountAsync(ct);
    }

    public async Task<IReadOnlyList<Proiect>> ObtineDupaClientAsync(Guid clientId, CancellationToken ct = default)
    {
        return await _context.Proiecte
            .Where(p => p.ClientId == clientId)
            .OrderByDescending(p => p.DataCreare)
            .ToListAsync(ct);
    }
}