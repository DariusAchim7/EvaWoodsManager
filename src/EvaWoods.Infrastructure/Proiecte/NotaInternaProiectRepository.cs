using EvaWoods.Domain.Proiecte;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Proiecte;

public class NotaInternaProiectRepository : INotaInternaProiectRepository
{
    private readonly ApplicationDbContext _context;

    public NotaInternaProiectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(NotaInternaProiect nota, CancellationToken ct = default)
    {
        await _context.NoteInterneProiect.AddAsync(nota, ct);
    }

    public async Task<IReadOnlyList<NotaInternaProiect>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        return await _context.NoteInterneProiect
            .Where(n => n.ProiectId == proiectId)
            .OrderByDescending(n => n.EsteFixata)
            .ThenBy(n => n.EsteRezolvata)
            .ThenByDescending(n => n.DataCreare)
            .ToListAsync(ct);
    }

    public async Task<NotaInternaProiect?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.NoteInterneProiect.FirstOrDefaultAsync(n => n.Id == id, ct);
    }
}