using EvaWoods.Domain.Abstractions;

namespace EvaWoods.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<int> SalveazaAsync(CancellationToken ct = default)
    {
        return _context.SaveChangesAsync(ct);
    }
}