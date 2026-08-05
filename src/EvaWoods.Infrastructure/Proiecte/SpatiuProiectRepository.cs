using EvaWoods.Domain.Proiecte;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Proiecte;

public class SpatiuProiectRepository : ISpatiuProiectRepository
{
    private readonly ApplicationDbContext _context;

    public SpatiuProiectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(SpatiuProiect spatiu, CancellationToken ct = default)
    {
        await _context.SpatiiProiect.AddAsync(spatiu, ct);
    }

    public async Task<IReadOnlyList<SpatiuProiect>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        return await _context.SpatiiProiect
            .Where(s => s.ProiectId == proiectId)
            .OrderBy(s => s.DataCreare)
            .ToListAsync(ct);
    }

    public async Task<SpatiuProiect?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.SpatiiProiect.FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public async Task StergeAsync(SpatiuProiect spatiu, CancellationToken ct = default)
    {
        _context.SpatiiProiect.Remove(spatiu);
        await Task.CompletedTask;
    }
}