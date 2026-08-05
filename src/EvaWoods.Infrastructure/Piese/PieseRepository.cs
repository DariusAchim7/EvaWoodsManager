using EvaWoods.Domain.Piese;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Piese;

public class PiesaRepository : IPiesaRepository
{
    private readonly ApplicationDbContext _context;

    public PiesaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(Piesa piesa, CancellationToken ct = default)
    {
        await _context.Piese.AddAsync(piesa, ct);
    }

    public async Task<IReadOnlyList<Piesa>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        return await _context.Piese
            .Where(p => p.ProiectId == proiectId)
            .OrderBy(p => p.DataCreare)
            .ToListAsync(ct);
    }

    public async Task<Piesa?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Piese.FirstOrDefaultAsync(p => p.Id == id, ct);
    }

    public async Task StergeAsync(Piesa piesa, CancellationToken ct = default)
    {
        _context.Piese.Remove(piesa);
        await Task.CompletedTask;
    }
}