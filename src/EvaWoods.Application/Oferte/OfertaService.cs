using EvaWoods.Domain.Abstractions;
using EvaWoods.Domain.Materiale;
using EvaWoods.Domain.Oferte;
using EvaWoods.Domain.Piese;
using EvaWoods.Domain.Produse;
using EvaWoods.Domain.Proiecte;

namespace EvaWoods.Application.Oferte;

public class OfertaService : IOfertaService
{
    private readonly ILinieOfertaRepository _linieRepository;
    private readonly IProdusRepository _produsRepository;
    private readonly ICalculMaterialRepository _calculMaterialRepository;
    private readonly IPiesaRepository _piesaRepository;
    private readonly IProiectRepository _proiectRepository;
    private readonly IUnitOfWork _unitOfWork;

    public OfertaService(
        ILinieOfertaRepository linieRepository,
        IProdusRepository produsRepository,
        ICalculMaterialRepository calculMaterialRepository,
        IPiesaRepository piesaRepository,
        IProiectRepository proiectRepository,
        IUnitOfWork unitOfWork)
    {
        _linieRepository = linieRepository;
        _produsRepository = produsRepository;
        _calculMaterialRepository = calculMaterialRepository;
        _piesaRepository = piesaRepository;
        _proiectRepository = proiectRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<LinieOfertaDto>> ObtineLiniiAsync(Guid proiectId, CancellationToken ct = default)
    {
        var linii = await _linieRepository.ObtineDupaProiectAsync(proiectId, ct);
        var produse = (await _produsRepository.ObtineToateAsync(ct)).ToDictionary(p => p.Id);

        return linii.Select(l =>
        {
            decimal? pretCurent = null;
            if (l.ProdusId.HasValue && produse.TryGetValue(l.ProdusId.Value, out var produs))
            {
                pretCurent = produs.PretEfectivCalcul;
            }

            return new LinieOfertaDto(
                l.Id, l.ProdusId, l.DescriereSnapshot, l.CategorieSnapshot, l.UMSnapshot,
                l.CantitateCalculata, l.CantitateOfertata, l.PretCostSnapshot, l.PretVanzareSnapshot,
                l.ProcentTva, l.Total, l.TotalCuTva, l.Nota,
                l.VizibilClient, l.EsteAutomat, pretCurent);
        }).ToList();
    }

    public async Task AdaugaLinieAsync(AdaugaLinieOfertaRequest request, CancellationToken ct = default)
    {
        var linii = await _linieRepository.ObtineDupaProiectAsync(request.ProiectId, ct);
        var ordine = linii.Count > 0 ? linii.Max(l => l.Ordine) + 1 : 0;

        var linie = LinieOferta.Creeaza(
            proiectId: request.ProiectId,
            produsId: request.ProdusId,
            descriere: request.Descriere,
            categorie: request.Categorie,
            um: request.UM,
            cantitateCalculata: request.CantitateOfertata,
            cantitateOfertata: request.CantitateOfertata,
            pretCost: request.PretCost,
            pretVanzare: request.PretVanzare,
            esteAutomat: false,
            ordine: ordine,
            procentTva: request.ProcentTva,
            vizibilClient: request.VizibilClient,
            nota: request.Nota);

        await _linieRepository.AdaugaAsync(linie, ct);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task ActualizeazaCantitateAsync(Guid linieId, decimal cantitate, CancellationToken ct = default)
    {
        var linie = await _linieRepository.ObtineDupaIdAsync(linieId, ct)
            ?? throw new InvalidOperationException("Linia nu a fost găsită.");

        linie.ActualizeazaCantitate(cantitate);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task ComutaVizibilClientAsync(Guid linieId, CancellationToken ct = default)
    {
        var linie = await _linieRepository.ObtineDupaIdAsync(linieId, ct)
            ?? throw new InvalidOperationException("Linia nu a fost găsită.");

        linie.ComutaVizibilClient();
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task StergeLinieAsync(Guid linieId, CancellationToken ct = default)
    {
        var linie = await _linieRepository.ObtineDupaIdAsync(linieId, ct);
        if (linie is null || linie.EsteAutomat) return;

        await _linieRepository.StergeAsync(linie, ct);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task SincronizeazaLiniiAutomateAsync(Guid proiectId, CancellationToken ct = default)
    {
        await _linieRepository.StergeToateAutomateAsync(proiectId, ct);

        var produse = (await _produsRepository.ObtineToateAsync(ct)).ToDictionary(p => p.Id);
        var linii = (await _linieRepository.ObtineDupaProiectAsync(proiectId, ct)).ToList();
        var ordine = linii.Count > 0 ? linii.Max(l => l.Ordine) + 1 : 0;

        async Task AdaugaLinieSiServiciuAsociat(Produs produsSursa, decimal cantitateCalculata, decimal cantitateOfertata)
        {
            var linie = LinieOferta.Creeaza(
                proiectId: proiectId,
                produsId: produsSursa.Id,
                descriere: produsSursa.Nume,
                categorie: produsSursa.Categorie,
                um: produsSursa.UM,
                cantitateCalculata: cantitateCalculata,
                cantitateOfertata: cantitateOfertata,
                pretCost: produsSursa.PretAchizitie,
                pretVanzare: produsSursa.PretEfectivCalcul,
                esteAutomat: true,
                ordine: ordine++);

            await _linieRepository.AdaugaAsync(linie, ct);

            if (produsSursa.ServiciuAsociatId.HasValue &&
                produse.TryGetValue(produsSursa.ServiciuAsociatId.Value, out var serviciu))
            {
                var linieServiciu = LinieOferta.Creeaza(
                    proiectId: proiectId,
                    produsId: serviciu.Id,
                    descriere: $"{serviciu.Nume} ({produsSursa.Nume})",
                    categorie: serviciu.Categorie,
                    um: serviciu.UM,
                    cantitateCalculata: cantitateOfertata,
                    cantitateOfertata: cantitateOfertata,
                    pretCost: serviciu.PretAchizitie,
                    pretVanzare: serviciu.PretEfectivCalcul,
                    esteAutomat: true,
                    ordine: ordine++);

                await _linieRepository.AdaugaAsync(linieServiciu, ct);
            }
        }

        // Materiale (PAL/MDF/PFL)
        var calcule = await _calculMaterialRepository.ObtineDupaProiectAsync(proiectId, ct);
        foreach (var calcul in calcule.Where(c => c.FoiEstimate > 0 || c.SuprafataPieseM2 > 0))
        {
            if (!produse.TryGetValue(calcul.MaterialProdusId, out var produs)) continue;

            var cantitateOfertata = produs.UM == UnitateMasura.Foaie
                ? calcul.FoiEstimate
                : Math.Ceiling(calcul.SuprafataPieseM2 * 1.05m * 100m) / 100m;

            var cantitateCalculata = produs.UM == UnitateMasura.Foaie ? calcul.FoiTeoretice : calcul.SuprafataPieseM2;

            await AdaugaLinieSiServiciuAsociat(produs, cantitateCalculata, cantitateOfertata);
        }

        // Cant
        var piese = await _piesaRepository.ObtineDupaProiectAsync(proiectId, ct);
        var grupuriCant = piese
            .Where(p => p.MaterialCantProdusId.HasValue)
            .GroupBy(p => p.MaterialCantProdusId!.Value);

        foreach (var grup in grupuriCant)
        {
            if (!produse.TryGetValue(grup.Key, out var produsCant)) continue;

            var necesarMl = grup.Sum(p => p.TotalCantMl);
            if (necesarMl <= 0) continue;

            var ofertaMl = Math.Ceiling(necesarMl * 1.05m / 5m) * 5m;

            await AdaugaLinieSiServiciuAsociat(produsCant, necesarMl, ofertaMl);
        }

        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<RezumatOfertaDto> ObtineRezumatAsync(Guid proiectId, CancellationToken ct = default)
    {
        var linii = await _linieRepository.ObtineDupaProiectAsync(proiectId, ct);
        var proiect = await _proiectRepository.ObtineDupaIdAsync(proiectId, ct);

        decimal SumaVanzare(CategorieProdus categorie) =>
            linii.Where(l => l.CategorieSnapshot == categorie).Sum(l => l.Total);

        var totalMateriale = SumaVanzare(CategorieProdus.Materiale) + SumaVanzare(CategorieProdus.Consumabile);
        var totalFeronerie = SumaVanzare(CategorieProdus.Feronerie);
        var totalServicii = SumaVanzare(CategorieProdus.Servicii);

        var costProduse = linii.Sum(l => l.CantitateOfertata * l.PretCostSnapshot);
        var vanzareProduse = totalMateriale + totalFeronerie + totalServicii;

        var procentManopera = proiect?.ProcentManopera ?? 0;
        var manopera = vanzareProduse * procentManopera / 100m;

        var costTotal = costProduse;
        var pretVanzareTotal = vanzareProduse + manopera;
        var profit = pretVanzareTotal - costTotal;
        var marja = pretVanzareTotal > 0 ? profit / pretVanzareTotal * 100m : 0;

        return new RezumatOfertaDto(
            totalMateriale, totalFeronerie, totalServicii, manopera,
            costTotal, pretVanzareTotal, profit, marja, vanzareProduse);
    }
}