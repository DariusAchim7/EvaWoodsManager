namespace EvaWoods.Domain.Clienti;

public interface IClientRepository
{
    Task AdaugaAsync(Client client, CancellationToken ct = default);
    Task<Client?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<Client>> ObtineTotiAsync(CancellationToken ct = default);
}