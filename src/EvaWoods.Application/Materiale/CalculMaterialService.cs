using System.Text.Json;
using EvaWoods.Domain.Abstractions;
using EvaWoods.Domain.Materiale;
using EvaWoods.Domain.Piese;
using EvaWoods.Domain.Produse;

namespace EvaWoods.Application.Materiale;

public class CalculMaterialService : ICalculMaterialService
{
    private const decimal MargineCuratareImplicitaMm = 10m;
    private const decimal KerfImplicitMm = 4m;

    private readonly ICalculMaterialRepository _calculRepository;
    private readonly IPiesaRepository _piesaRepository;
    private readonly IProdusRepository _produsRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CalculMaterialService(
        ICalculMaterialRepository calculRepository,
        IPiesaRepository piesaRepository,
        IProdusRepository produsRepository,
        IUnitOfWork unitOfWork)
    {
        _calculRepository = calculRepository;
        _piesaRepository = piesaRepository;
        _produsRepository = produsRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task RecalculeazaAsync(Guid proiectId, CancellationToken ct = default)
    {
        var piese = await _piesaRepository.ObtineDupaProiectAsync(proiectId, ct);
        var produse = (await _produsRepository.ObtineToateAsync(ct)).ToDictionary(p => p.Id);
        var calculeExistente = (await _calculRepository.ObtineDupaProiectAsync(proiectId, ct)).ToList();

        var grupuri = piese
            .Where(p => p.MaterialProdusId.HasValue)
            .GroupBy(p => p.MaterialProdusId!.Value)
            .ToList();

        // Șterge calculele pentru materiale care nu mai sunt folosite
        var materialeFolosite = grupuri.Select(g => g.Key).ToHashSet();
        foreach (var vechi in calculeExistente.Where(c => !materialeFolosite.Contains(c.MaterialProdusId)).ToList())
        {
            await _calculRepository.StergeAsync(vechi, ct);
            calculeExistente.Remove(vechi);
        }

        foreach (var grup in grupuri)
        {
            var calcul = calculeExistente.FirstOrDefault(c => c.MaterialProdusId == grup.Key);
            if (calcul is null)
            {
                calcul = CalculMaterial.Creeaza(proiectId, grup.Key);
                await _calculRepository.AdaugaAsync(calcul, ct);
            }

            var pieseGrup = grup.ToList();
            var numarTipuri = pieseGrup.Count;
            var numarBucati = pieseGrup.Sum(p => p.Cantitate);
            var suprafataM2 = pieseGrup.Sum(p => p.SuprafataM2);

            if (!produse.TryGetValue(grup.Key, out var produs) ||
                produs.LungimeFoaieMm is not > 0 || produs.LatimeFoaieMm is not > 0)
            {
                calcul.ActualizeazaRezultat(
                    numarTipuri, numarBucati, suprafataM2,
                    foiTeoretice: 0, foiEstimate: 0, utilizareProcent: 0,
                    layoutJson: null,
                    erori: "Materialul nu are dimensiunea plăcii configurată în catalog.");
                continue;
            }

            var suprafataFoaieM2 = produs.LungimeFoaieMm.Value * produs.LatimeFoaieMm.Value / 1_000_000m;
            var foiTeoretice = suprafataFoaieM2 > 0 ? Math.Round(suprafataM2 / suprafataFoaieM2, 2) : 0;

            // Regula de rotire: materialul trebuie să permită rotirea, iar piesa fie nu are fibră
            // direcționată, fie permite explicit rotirea.
            var pieseDeAsezat = new List<PiesaDeAsezat>();
            foreach (var piesa in pieseGrup)
            {
                var poateRoti = produs.PermiteRotire &&
                    (piesa.DirectieFibra == DirectieFibra.Fara || piesa.PermiteRotire);

                for (var i = 0; i < piesa.Cantitate; i++)
                {
                    pieseDeAsezat.Add(new PiesaDeAsezat(piesa.Id, piesa.Nume, piesa.LungimeMm, piesa.LatimeMm, poateRoti));
                }
            }

            var rezultat = OptimizatorDebitare.Optimizeaza(
                pieseDeAsezat,
                produs.LungimeFoaieMm.Value,
                produs.LatimeFoaieMm.Value,
                produs.KerfMm ?? KerfImplicitMm,
                MargineCuratareImplicitaMm);

            var erori = rezultat.PieseNeplasabile.Count > 0
                ? string.Join(" | ", rezultat.PieseNeplasabile)
                : null;

            calcul.ActualizeazaRezultat(
                numarTipuri, numarBucati, suprafataM2,
                foiTeoretice,
                rezultat.NumarFoi,
                rezultat.UtilizareProcent,
                JsonSerializer.Serialize(rezultat.Foi),
                erori);
        }

        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<IReadOnlyList<CalculMaterialDto>> ObtineCalculeAsync(Guid proiectId, CancellationToken ct = default)
    {
        var calcule = await _calculRepository.ObtineDupaProiectAsync(proiectId, ct);
        var produse = (await _produsRepository.ObtineToateAsync(ct)).ToDictionary(p => p.Id);

        return calcule
            .Select(c =>
            {
                produse.TryGetValue(c.MaterialProdusId, out var produs);
                return new CalculMaterialDto(
                    c.Id,
                    c.MaterialProdusId,
                    produs?.Nume ?? "Material necunoscut",
                    produs?.Subcategorie,
                    produs?.GrosimeMm,
                    produs?.LungimeFoaieMm,
                    produs?.LatimeFoaieMm,
                    c.NumarTipuriPiese,
                    c.NumarBucati,
                    c.SuprafataPieseM2,
                    c.FoiTeoretice,
                    c.FoiEstimate,
                    c.UtilizareProcent,
                    c.PierdereProcent,
                    c.LayoutJson,
                    c.Erori,
                    c.Status,
                    c.DataCalcul,
                    produs?.PretEfectivCalcul);
            })
            .OrderByDescending(c => c.SuprafataPieseM2)
            .ToList();
    }
}