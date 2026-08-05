using EvaWoods.Domain.Abstractions;
using EvaWoods.Domain.Clienti;
using EvaWoods.Domain.Proiecte;

namespace EvaWoods.Application.Clienti;

public class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IProiectRepository _proiectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ClientService(
        IClientRepository clientRepository,
        IProiectRepository proiectRepository,
        IUnitOfWork unitOfWork)
    {
        _clientRepository = clientRepository;
        _proiectRepository = proiectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreeazaClientAsync(CreeazaClientRequest request, CancellationToken ct = default)
    {
        var client = Client.Creeaza(
            nume: request.Nume,
            telefon: request.Telefon,
            localitate: request.Localitate,
            email: request.Email,
            judet: request.Judet,
            status: request.Status);

        await _clientRepository.AdaugaAsync(client, ct);
        await _unitOfWork.SalveazaAsync(ct);

        return client.Id;
    }

    public async Task<IReadOnlyList<ClientDto>> ObtineListaClientiAsync(CancellationToken ct = default)
    {
        var clienti = await _clientRepository.ObtineTotiAsync(ct);
        var proiecte = await _proiectRepository.ObtineToateAsync(ct);

        var numarProiecteDupaClient = proiecte
            .GroupBy(p => p.ClientId)
            .ToDictionary(g => g.Key, g => g.Count());

        return clienti.Select(c => MapeazaLaDto(c, numarProiecteDupaClient.GetValueOrDefault(c.Id))).ToList();
    }

    public async Task<ClientDto?> ObtineClientDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        var client = await _clientRepository.ObtineDupaIdAsync(id, ct);
        if (client is null) return null;

        var proiecte = await _proiectRepository.ObtineDupaClientAsync(id, ct);

        return MapeazaLaDto(client, proiecte.Count);
    }

    public async Task AdaugaNotaAsync(Guid clientId, string nota, CancellationToken ct = default)
    {
        var client = await _clientRepository.ObtineDupaIdAsync(clientId, ct)
            ?? throw new InvalidOperationException("Clientul nu a fost găsit.");

        client.AdaugaNota(nota);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task ActualizeazaClientAsync(ActualizeazaClientRequest request, CancellationToken ct = default)
    {
        var client = await _clientRepository.ObtineDupaIdAsync(request.Id, ct)
            ?? throw new InvalidOperationException("Clientul nu a fost găsit.");

        client.Actualizeaza(
            nume: request.Nume,
            telefon: request.Telefon,
            localitate: request.Localitate,
            email: request.Email,
            judet: request.Judet,
            status: request.Status);

        await _unitOfWork.SalveazaAsync(ct);
    }

    private static ClientDto MapeazaLaDto(Client client, int numarProiecte) => new(
        client.Id,
        client.Nume,
        client.Telefon,
        client.Email,
        client.Localitate,
        client.Judet,
        client.Status,
        client.DataUltimeiActivitati,
        numarProiecte,
        client.Note);
}