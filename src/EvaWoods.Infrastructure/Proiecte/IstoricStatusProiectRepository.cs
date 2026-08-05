using EvaWoods.Domain.Proiecte;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Proiecte;

public class IstoricStatusProiectRepository : IIstoricStatusProiectRepository
{
    private readonly ApplicationDbContext _context;

    public IstoricStatusProiectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(IstoricStatusProiect intrare, CancellationToken ct = default)
    {
        await _context.IstoricStatusProiect.AddAsync(intrare, ct);
    }

    public async Task<IReadOnlyList<IstoricStatusProiect>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        return await _context.IstoricStatusProiect
            .Where(i => i.ProiectId == proiectId)
            .OrderByDescending(i => i.DataSchimbare)
            .ToListAsync(ct);
    }
}