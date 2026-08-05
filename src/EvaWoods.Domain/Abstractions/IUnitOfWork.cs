namespace EvaWoods.Domain.Abstractions;

public interface IUnitOfWork
{
    Task<int> SalveazaAsync(CancellationToken ct = default);
}