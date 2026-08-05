using EvaWoods.Domain.Abstractions;
using EvaWoods.Domain.Cheltuieli;
using EvaWoods.Domain.Produse;

namespace EvaWoods.Application.Produse;

public class ProdusService : IProdusService
{
    private readonly IProdusRepository _produsRepository;
    private readonly IFurnizorRepository _furnizorRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProdusService(
        IProdusRepository produsRepository,
        IFurnizorRepository furnizorRepository,
        IUnitOfWork unitOfWork)
    {
        _produsRepository = produsRepository;
        _furnizorRepository = furnizorRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreeazaProdusAsync(CreeazaProdusRequest request, CancellationToken ct = default)
    {
        var numarUrmator = await _produsRepository.ObtineNumarPeCategorieAsync(request.Categorie, ct) + 1;
        var cod = $"{PrefixCategorie(request.Categorie)}-{numarUrmator:000}";

        var produs = Produs.Creeaza(
            cod: cod,
            nume: request.Nume,
            categorie: request.Categorie,
            um: request.UM,
            pretAchizitie: request.PretAchizitie,
            pretCalcul: request.PretCalcul,
            procentTva: request.ProcentTva,
            subcategorie: request.Subcategorie,
            furnizorId: request.FurnizorId,
            observatii: request.Observatii,
            lungimeFoaieMm: request.LungimeFoaieMm,
            latimeFoaieMm: request.LatimeFoaieMm,
            grosimeMm: request.GrosimeMm,
            permiteRotire: request.PermiteRotire,
            kerfMm: request.KerfMm,
            serviciuAsociatId: request.ServiciuAsociatId);

        await _produsRepository.AdaugaAsync(produs, ct);
        await _unitOfWork.SalveazaAsync(ct);

        return produs.Id;
    }

    public async Task<IReadOnlyList<ProdusDto>> ObtineToateProduseleAsync(CancellationToken ct = default)
    {
        var produse = await _produsRepository.ObtineToateAsync(ct);
        var furnizori = await _furnizorRepository.ObtineToateAsync(ct);
        var furnizoriDupaId = furnizori.ToDictionary(f => f.Id);
        var produseDupaId = produse.ToDictionary(p => p.Id);

        return produse.Select(p =>
        {
            string? numeFurnizor = null;
            if (p.FurnizorId.HasValue && furnizoriDupaId.TryGetValue(p.FurnizorId.Value, out var furnizor))
            {
                numeFurnizor = furnizor.Nume;
            }

            string? numeServiciu = null;
            if (p.ServiciuAsociatId.HasValue && produseDupaId.TryGetValue(p.ServiciuAsociatId.Value, out var serviciu))
            {
                numeServiciu = serviciu.Nume;
            }

            return new ProdusDto(
                p.Id, p.Cod, p.Nume, p.Categorie, p.Subcategorie, p.UM,
                p.PretAchizitie, p.PretCalcul, p.ProcentTva,
                p.FurnizorId, numeFurnizor, p.EsteActiv, p.DataUltimeiActualizariPret, p.Observatii,
                p.LungimeFoaieMm, p.LatimeFoaieMm, p.GrosimeMm, p.PermiteRotire, p.KerfMm,
                p.ServiciuAsociatId, numeServiciu);
        }).ToList();
    }

    public async Task ActualizeazaProdusAsync(ActualizeazaProdusRequest request, CancellationToken ct = default)
    {
        var produs = await _produsRepository.ObtineDupaIdAsync(request.Id, ct)
            ?? throw new InvalidOperationException("Produsul nu a fost găsit.");

        produs.Actualizeaza(
            nume: request.Nume,
            categorie: request.Categorie,
            subcategorie: request.Subcategorie,
            um: request.UM,
            pretAchizitie: request.PretAchizitie,
            pretCalcul: request.PretCalcul,
            procentTva: request.ProcentTva,
            furnizorId: request.FurnizorId,
            observatii: request.Observatii,
            lungimeFoaieMm: request.LungimeFoaieMm,
            latimeFoaieMm: request.LatimeFoaieMm,
            grosimeMm: request.GrosimeMm,
            permiteRotire: request.PermiteRotire,
            kerfMm: request.KerfMm,
            serviciuAsociatId: request.ServiciuAsociatId);

        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task ComutaActivProdusAsync(Guid id, CancellationToken ct = default)
    {
        var produs = await _produsRepository.ObtineDupaIdAsync(id, ct)
            ?? throw new InvalidOperationException("Produsul nu a fost găsit.");

        produs.ComutaActiv();
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task StergeProdusAsync(Guid id, CancellationToken ct = default)
    {
        var produs = await _produsRepository.ObtineDupaIdAsync(id, ct);
        if (produs is null) return;

        await _produsRepository.StergeAsync(produs, ct);
        await _unitOfWork.SalveazaAsync(ct);
    }

    private static string PrefixCategorie(CategorieProdus categorie) => categorie switch
    {
        CategorieProdus.Materiale => "MAT",
        CategorieProdus.Consumabile => "CON",
        CategorieProdus.Feronerie => "FER",
        CategorieProdus.Servicii => "SER",
        _ => "PRD"
    };
}