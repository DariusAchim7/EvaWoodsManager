using EvaWoods.Domain.Materiale;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Materiale;

public class CalculMaterialRepository : ICalculMaterialRepository
{
    private readonly ApplicationDbContext _context;

    public CalculMaterialRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(CalculMaterial calcul, CancellationToken ct = default)
    {
        await _context.CalculeMateriale.AddAsync(calcul, ct);
    }

    public async Task<IReadOnlyList<CalculMaterial>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        return await _context.CalculeMateriale
            .Where(c => c.ProiectId == proiectId)
            .ToListAsync(ct);
    }

    public async Task StergeAsync(CalculMaterial calcul, CancellationToken ct = default)
    {
        _context.CalculeMateriale.Remove(calcul);
        await Task.CompletedTask;
    }
}