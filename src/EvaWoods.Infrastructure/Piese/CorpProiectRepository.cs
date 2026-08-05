using EvaWoods.Domain.Piese;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Piese;

public class CorpProiectRepository : ICorpProiectRepository
{
    private readonly ApplicationDbContext _context;

    public CorpProiectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(CorpProiect corp, CancellationToken ct = default)
    {
        await _context.CorpuriProiect.AddAsync(corp, ct);
    }

    public async Task<IReadOnlyList<CorpProiect>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        return await _context.CorpuriProiect
            .Where(c => c.ProiectId == proiectId)
            .OrderBy(c => c.Nume)
            .ToListAsync(ct);
    }
}