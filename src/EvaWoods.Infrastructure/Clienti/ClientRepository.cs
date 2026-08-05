using EvaWoods.Domain.Clienti;
using EvaWoods.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EvaWoods.Infrastructure.Clienti;

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;

    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AdaugaAsync(Client client, CancellationToken ct = default)
    {
        await _context.Clienti.AddAsync(client, ct);
    }

    public async Task<Client?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Clienti.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<IReadOnlyList<Client>> ObtineTotiAsync(CancellationToken ct = default)
    {
        return await _context.Clienti
            .OrderByDescending(c => c.DataCreare)
            .ToListAsync(ct);
    }
}