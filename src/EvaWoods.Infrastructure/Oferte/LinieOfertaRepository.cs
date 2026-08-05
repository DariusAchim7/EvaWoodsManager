using EvaWoods.Domain.Oferte;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Oferte;

public class LinieOfertaRepository : ILinieOfertaRepository
{
    private readonly ApplicationDbContext _context;

    public LinieOfertaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(LinieOferta linie, CancellationToken ct = default)
    {
        await _context.LiniiOferta.AddAsync(linie, ct);
    }

    public async Task<IReadOnlyList<LinieOferta>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        return await _context.LiniiOferta
            .Where(l => l.ProiectId == proiectId)
            .OrderBy(l => l.Ordine)
            .ToListAsync(ct);
    }

    public async Task<LinieOferta?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.LiniiOferta.FirstOrDefaultAsync(l => l.Id == id, ct);
    }

    public async Task StergeAsync(LinieOferta linie, CancellationToken ct = default)
    {
        _context.LiniiOferta.Remove(linie);
        await Task.CompletedTask;
    }

    public async Task StergeToateAutomateAsync(Guid proiectId, CancellationToken ct = default)
    {
        var automate = await _context.LiniiOferta
            .Where(l => l.ProiectId == proiectId && l.EsteAutomat)
            .ToListAsync(ct);

        _context.LiniiOferta.RemoveRange(automate);
    }
}