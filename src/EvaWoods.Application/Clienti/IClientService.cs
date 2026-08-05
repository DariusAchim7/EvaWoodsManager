namespace EvaWoods.Application.Clienti;

public interface IClientService
{
    Task<Guid> CreeazaClientAsync(CreeazaClientRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<ClientDto>> ObtineListaClientiAsync(CancellationToken ct = default);
    Task<ClientDto?> ObtineClientDupaIdAsync(Guid id, CancellationToken ct = default);
    Task AdaugaNotaAsync(Guid clientId, string nota, CancellationToken ct = default);
    Task ActualizeazaClientAsync(ActualizeazaClientRequest request, CancellationToken ct = default);
}