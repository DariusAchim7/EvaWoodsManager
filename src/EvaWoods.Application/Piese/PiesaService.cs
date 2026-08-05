using EvaWoods.Domain.Abstractions;
using EvaWoods.Domain.Piese;
using EvaWoods.Domain.Produse;
using EvaWoods.Domain.Materiale;

namespace EvaWoods.Application.Piese;

public class PiesaService : IPiesaService
{
    private readonly IPiesaRepository _piesaRepository;
    private readonly ICorpProiectRepository _corpRepository;
    private readonly IProdusRepository _produsRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICalculMaterialRepository _calculMaterialRepository;

    public PiesaService(
        IPiesaRepository piesaRepository,
        ICorpProiectRepository corpRepository,
        IProdusRepository produsRepository,
        IUnitOfWork unitOfWork,
        ICalculMaterialRepository calculMaterialRepository)
    {
        _piesaRepository = piesaRepository;
        _corpRepository = corpRepository;
        _produsRepository = produsRepository;
        _unitOfWork = unitOfWork;
        _calculMaterialRepository = calculMaterialRepository;
    }

   public async Task AdaugaPieseAsync(IReadOnlyList<CreeazaPiesaRequest> requests, CancellationToken ct = default)
    {
        if (requests.Count == 0) return;

        var existente = (await _piesaRepository.ObtineDupaProiectAsync(requests[0].ProiectId, ct)).ToList();

        foreach (var request in requests)
        {
            var identica = existente.FirstOrDefault(p => EstePiesaIdentica(p, request));

            if (identica is not null)
            {
                identica.AdaugaCantitate(request.Cantitate);
                continue;
            }

            var piesa = Piesa.Creeaza(
                proiectId: request.ProiectId,
                lungimeMm: request.LungimeMm,
                latimeMm: request.LatimeMm,
                cantitate: request.Cantitate,
                nume: request.Nume,
                corpId: request.CorpId,
                materialProdusId: request.MaterialProdusId,
                grosimeMm: request.GrosimeMm,
                cantLaturiLungi: request.CantLaturiLungi,
                cantLaturiScurte: request.CantLaturiScurte,
                materialCantProdusId: request.MaterialCantProdusId,
                directieFibra: request.DirectieFibra,
                permiteRotire: request.PermiteRotire,
                observatii: request.Observatii);

            await _piesaRepository.AdaugaAsync(piesa, ct);
            existente.Add(piesa);
        }

        await InvalideazaCalculele(requests[0].ProiectId, ct);
        await _unitOfWork.SalveazaAsync(ct);
    }

    private static bool EstePiesaIdentica(Piesa piesa, CreeazaPiesaRequest request) =>
        piesa.LungimeMm == request.LungimeMm &&
        piesa.LatimeMm == request.LatimeMm &&
        piesa.MaterialProdusId == request.MaterialProdusId &&
        piesa.CorpId == request.CorpId &&
        piesa.CantLaturiLungi == request.CantLaturiLungi &&
        piesa.CantLaturiScurte == request.CantLaturiScurte &&
        piesa.MaterialCantProdusId == request.MaterialCantProdusId &&
        piesa.DirectieFibra == request.DirectieFibra &&
        piesa.GrosimeMm == request.GrosimeMm &&
        piesa.PermiteRotire == request.PermiteRotire;

    public async Task<IReadOnlyList<PiesaDto>> ObtinePieseDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        var piese = await _piesaRepository.ObtineDupaProiectAsync(proiectId, ct);
        var corpuri = (await _corpRepository.ObtineDupaProiectAsync(proiectId, ct)).ToDictionary(c => c.Id);
        var produse = (await _produsRepository.ObtineToateAsync(ct)).ToDictionary(p => p.Id);

        return piese.Select(p => new PiesaDto(
            p.Id,
            p.Nume,
            p.LungimeMm,
            p.LatimeMm,
            p.Cantitate,
            p.CorpId,
            p.CorpId.HasValue && corpuri.TryGetValue(p.CorpId.Value, out var corp) ? corp.Nume : null,
            p.MaterialProdusId,
            p.MaterialProdusId.HasValue && produse.TryGetValue(p.MaterialProdusId.Value, out var material) ? material.Nume : null,
            p.GrosimeMm,
            p.CantLaturiLungi,
            p.CantLaturiScurte,
            p.MaterialCantProdusId,
            p.MaterialCantProdusId.HasValue && produse.TryGetValue(p.MaterialCantProdusId.Value, out var materialCant) ? materialCant.Nume : null,
            p.DirectieFibra,
            p.PermiteRotire,
            p.Observatii,
            p.TotalCantMl,
            p.SuprafataM2)).ToList();
    }

    private async Task InvalideazaCalculele(Guid proiectId, CancellationToken ct)
    {
        var calcule = await _calculMaterialRepository.ObtineDupaProiectAsync(proiectId, ct);
        foreach (var calcul in calcule)
        {
            calcul.MarcheazaNecesitaRecalculare();
        }
    }

    public async Task<int> CombinaPieseleIdenticeAsync(Guid proiectId, CancellationToken ct = default)
    {
        var piese = await _piesaRepository.ObtineDupaProiectAsync(proiectId, ct);

        var grupuri = piese.GroupBy(p => (
            p.LungimeMm, p.LatimeMm, p.MaterialProdusId, p.CorpId,
            p.CantLaturiLungi, p.CantLaturiScurte, p.MaterialCantProdusId,
            p.DirectieFibra, p.GrosimeMm, p.PermiteRotire));

        var numarCombinat = 0;

        foreach (var grup in grupuri.Where(g => g.Count() > 1))
        {
            var pastrata = grup.First();

            foreach (var duplicat in grup.Skip(1))
            {
                pastrata.AdaugaCantitate(duplicat.Cantitate);
                await _piesaRepository.StergeAsync(duplicat, ct);
                numarCombinat++;
            }
        }

        if (numarCombinat > 0)
        {
            await InvalideazaCalculele(proiectId, ct);
            await _unitOfWork.SalveazaAsync(ct);
        }

        return numarCombinat;
    }

    public async Task ActualizeazaPiesaAsync(ActualizeazaPiesaRequest request, CancellationToken ct = default)
    {
        var piesa = await _piesaRepository.ObtineDupaIdAsync(request.Id, ct)
            ?? throw new InvalidOperationException("Piesa nu a fost găsită.");

        piesa.Actualizeaza(
            lungimeMm: request.LungimeMm,
            latimeMm: request.LatimeMm,
            cantitate: request.Cantitate,
            nume: request.Nume,
            corpId: request.CorpId,
            materialProdusId: request.MaterialProdusId,
            grosimeMm: request.GrosimeMm,
            cantLaturiLungi: request.CantLaturiLungi,
            cantLaturiScurte: request.CantLaturiScurte,
            materialCantProdusId: request.MaterialCantProdusId,
            directieFibra: request.DirectieFibra,
            permiteRotire: request.PermiteRotire,
            observatii: request.Observatii);

        await InvalideazaCalculele(piesa.ProiectId, ct);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<Guid> DuplicaPiesaAsync(Guid piesaId, CancellationToken ct = default)
    {
        var originala = await _piesaRepository.ObtineDupaIdAsync(piesaId, ct)
            ?? throw new InvalidOperationException("Piesa nu a fost găsită.");

        var copie = Piesa.Creeaza(
            proiectId: originala.ProiectId,
            lungimeMm: originala.LungimeMm,
            latimeMm: originala.LatimeMm,
            cantitate: originala.Cantitate,
            nume: originala.Nume,
            corpId: originala.CorpId,
            materialProdusId: originala.MaterialProdusId,
            grosimeMm: originala.GrosimeMm,
            cantLaturiLungi: originala.CantLaturiLungi,
            cantLaturiScurte: originala.CantLaturiScurte,
            materialCantProdusId: originala.MaterialCantProdusId,
            directieFibra: originala.DirectieFibra,
            permiteRotire: originala.PermiteRotire,
            observatii: originala.Observatii);

        await _piesaRepository.AdaugaAsync(copie, ct);
        await InvalideazaCalculele(originala.ProiectId, ct);
        await _unitOfWork.SalveazaAsync(ct);

        return copie.Id;
    }

    public async Task StergePiesaAsync(Guid piesaId, CancellationToken ct = default)
    {
        var piesa = await _piesaRepository.ObtineDupaIdAsync(piesaId, ct);
        if (piesa is null) return;

        await _piesaRepository.StergeAsync(piesa, ct);
        await InvalideazaCalculele(piesa.ProiectId, ct);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<Guid> AdaugaCorpAsync(Guid proiectId, string nume, CancellationToken ct = default)
    {
        var corp = CorpProiect.Creeaza(proiectId, nume);
        await _corpRepository.AdaugaAsync(corp, ct);
        await _unitOfWork.SalveazaAsync(ct);
        return corp.Id;
    }

    public async Task<IReadOnlyList<CorpProiectDto>> ObtineCorpuriDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        var corpuri = await _corpRepository.ObtineDupaProiectAsync(proiectId, ct);
        return corpuri.Select(c => new CorpProiectDto(c.Id, c.Nume)).ToList();
    }
}