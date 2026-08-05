using EvaWoods.Domain.Cheltuieli;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Cheltuieli;

public class CheltuialaRepository : ICheltuialaRepository
{
    private readonly ApplicationDbContext _context;

    public CheltuialaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(Cheltuiala cheltuiala, CancellationToken ct = default)
    {
        await _context.Cheltuieli.AddAsync(cheltuiala, ct);
    }

    public async Task<IReadOnlyList<Cheltuiala>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        return await _context.Cheltuieli
            .Where(c => c.ProiectId == proiectId)
            .OrderByDescending(c => c.Data)
            .ToListAsync(ct);
    }

    public async Task<Cheltuiala?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Cheltuieli.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task StergeAsync(Cheltuiala cheltuiala, CancellationToken ct = default)
    {
        _context.Cheltuieli.Remove(cheltuiala);
        await Task.CompletedTask;
    }
}