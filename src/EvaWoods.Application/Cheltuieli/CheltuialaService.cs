using EvaWoods.Domain.Abstractions;
using EvaWoods.Domain.Cheltuieli;
using EvaWoods.Domain.Proiecte;

namespace EvaWoods.Application.Cheltuieli;

public class CheltuialaService : ICheltuialaService
{
    private readonly ICheltuialaRepository _cheltuialaRepository;
    private readonly IFurnizorRepository _furnizorRepository;
    private readonly IProiectRepository _proiectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CheltuialaService(
        ICheltuialaRepository cheltuialaRepository,
        IFurnizorRepository furnizorRepository,
        IProiectRepository proiectRepository,
        IUnitOfWork unitOfWork)
    {
        _cheltuialaRepository = cheltuialaRepository;
        _furnizorRepository = furnizorRepository;
        _proiectRepository = proiectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreeazaCheltuialaAsync(CreeazaCheltuialaRequest request, CancellationToken ct = default)
    {
        var cheltuiala = Cheltuiala.Creeaza(
            proiectId: request.ProiectId,
            descriere: request.Descriere,
            categorie: request.Categorie,
            valoareFaraTva: request.ValoareFaraTva,
            procentTva: request.ProcentTva,
            data: request.Data,
            status: request.Status,
            subcategorie: request.Subcategorie,
            furnizorId: request.FurnizorId,
            numarFactura: request.NumarFactura,
            metodaPlata: request.MetodaPlata,
            statusPlata: request.StatusPlata,
            includeInBuget: request.IncludeInBuget,
            observatii: request.Observatii);

        await _cheltuialaRepository.AdaugaAsync(cheltuiala, ct);
        await _unitOfWork.SalveazaAsync(ct);

        return cheltuiala.Id;
    }

    public async Task<IReadOnlyList<CheltuialaDto>> ObtineCheltuieliDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        var cheltuieli = await _cheltuialaRepository.ObtineDupaProiectAsync(proiectId, ct);
        var furnizori = await _furnizorRepository.ObtineToateAsync(ct);
        var furnizoriDupaId = furnizori.ToDictionary(f => f.Id);

        return cheltuieli.Select(c => MapeazaLaDto(c, furnizoriDupaId)).ToList();
    }

    public async Task<CheltuialaDto?> ObtineCheltuialaDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        var cheltuiala = await _cheltuialaRepository.ObtineDupaIdAsync(id, ct);
        if (cheltuiala is null) return null;

        var furnizori = await _furnizorRepository.ObtineToateAsync(ct);
        var furnizoriDupaId = furnizori.ToDictionary(f => f.Id);

        return MapeazaLaDto(cheltuiala, furnizoriDupaId);
    }

    public async Task ActualizeazaCheltuialaAsync(ActualizeazaCheltuialaRequest request, CancellationToken ct = default)
    {
        var cheltuiala = await _cheltuialaRepository.ObtineDupaIdAsync(request.Id, ct)
            ?? throw new InvalidOperationException("Cheltuiala nu a fost găsită.");

        cheltuiala.Actualizeaza(
            descriere: request.Descriere,
            categorie: request.Categorie,
            valoareFaraTva: request.ValoareFaraTva,
            procentTva: request.ProcentTva,
            data: request.Data,
            subcategorie: request.Subcategorie,
            furnizorId: request.FurnizorId,
            numarFactura: request.NumarFactura,
            metodaPlata: request.MetodaPlata,
            includeInBuget: request.IncludeInBuget,
            observatii: request.Observatii);

        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task MarcheazaCaPlatitaAsync(Guid cheltuialaId, CancellationToken ct = default)
    {
        var cheltuiala = await _cheltuialaRepository.ObtineDupaIdAsync(cheltuialaId, ct)
            ?? throw new InvalidOperationException("Cheltuiala nu a fost găsită.");

        cheltuiala.MarcheazaCaPlatita();
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<Guid> DuplicaCheltuialaAsync(Guid cheltuialaId, CancellationToken ct = default)
    {
        var originala = await _cheltuialaRepository.ObtineDupaIdAsync(cheltuialaId, ct)
            ?? throw new InvalidOperationException("Cheltuiala nu a fost găsită.");

        var copie = Cheltuiala.Creeaza(
            proiectId: originala.ProiectId,
            descriere: $"{originala.Descriere} (copie)",
            categorie: originala.Categorie,
            valoareFaraTva: originala.ValoareFaraTva,
            procentTva: originala.ProcentTva,
            data: originala.Data,
            status: StatusCheltuiala.Ciorna,
            subcategorie: originala.Subcategorie,
            furnizorId: originala.FurnizorId,
            numarFactura: null,
            metodaPlata: originala.MetodaPlata,
            statusPlata: StatusPlata.InAsteptare,
            includeInBuget: originala.IncludeInBuget,
            observatii: originala.Observatii);

        await _cheltuialaRepository.AdaugaAsync(copie, ct);
        await _unitOfWork.SalveazaAsync(ct);

        return copie.Id;
    }

    public async Task StergeCheltuialaAsync(Guid cheltuialaId, CancellationToken ct = default)
    {
        var cheltuiala = await _cheltuialaRepository.ObtineDupaIdAsync(cheltuialaId, ct);
        if (cheltuiala is null) return;

        await _cheltuialaRepository.StergeAsync(cheltuiala, ct);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<Guid> AdaugaFurnizorAsync(string nume, CancellationToken ct = default)
    {
        var furnizor = Furnizor.Creeaza(nume);
        await _furnizorRepository.AdaugaAsync(furnizor, ct);
        await _unitOfWork.SalveazaAsync(ct);
        return furnizor.Id;
    }

    public async Task<IReadOnlyList<FurnizorDto>> ObtineFurnizoriAsync(CancellationToken ct = default)
    {
        var furnizori = await _furnizorRepository.ObtineToateAsync(ct);
        return furnizori.Select(f => new FurnizorDto(f.Id, f.Nume)).ToList();
    }

    public async Task<decimal?> ObtineBugetRamasAsync(Guid proiectId, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(proiectId, ct);
        if (proiect?.BugetAlocat is null) return null;

        var cheltuieli = await _cheltuialaRepository.ObtineDupaProiectAsync(proiectId, ct);
        var cheltuit = cheltuieli
            .Where(c => c.Status == StatusCheltuiala.Confirmata && c.IncludeInBuget)
            .Sum(c => c.Total);

        return proiect.BugetAlocat.Value - cheltuit;
    }

    private static CheltuialaDto MapeazaLaDto(Cheltuiala c, Dictionary<Guid, Furnizor> furnizoriDupaId)
    {
        string? numeFurnizor = null;
        if (c.FurnizorId.HasValue && furnizoriDupaId.TryGetValue(c.FurnizorId.Value, out var furnizor))
        {
            numeFurnizor = furnizor.Nume;
        }

        return new CheltuialaDto(
            c.Id,
            c.Descriere,
            c.Categorie,
            c.Subcategorie,
            c.FurnizorId,
            numeFurnizor,
            c.Data,
            c.NumarFactura,
            c.ValoareFaraTva,
            c.ProcentTva,
            c.Tva,
            c.Total,
            c.MetodaPlata,
            c.StatusPlata,
            c.Status,
            c.IncludeInBuget,
            c.Observatii);
    }
}