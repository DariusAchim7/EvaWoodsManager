using EvaWoods.Domain.Abstractions;
using EvaWoods.Domain.Clienti;
using EvaWoods.Domain.Proiecte;

namespace EvaWoods.Application.Proiecte;

public class ProiectService : IProiectService
{
    private readonly IProiectRepository _proiectRepository;
    private readonly IClientRepository _clientRepository;
    private readonly IIstoricStatusProiectRepository _istoricStatusRepository;
    private readonly ISpatiuProiectRepository _spatiuRepository;
    private readonly INotaInternaProiectRepository _notaInternaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProiectService(
        IProiectRepository proiectRepository,
        IClientRepository clientRepository,
        IIstoricStatusProiectRepository istoricStatusRepository,
        ISpatiuProiectRepository spatiuRepository,
        INotaInternaProiectRepository notaInternaRepository,
        IUnitOfWork unitOfWork)
    {
        _proiectRepository = proiectRepository;
        _clientRepository = clientRepository;
        _istoricStatusRepository = istoricStatusRepository;
        _spatiuRepository = spatiuRepository;
        _notaInternaRepository = notaInternaRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> CreeazaProiectAsync(CreeazaProiectRequest request, CancellationToken ct = default)
    {
        var client = await _clientRepository.ObtineDupaIdAsync(request.ClientId, ct)
            ?? throw new InvalidOperationException("Clientul selectat nu a fost găsit.");

        var numarUrmator = await _proiectRepository.ObtineNumarTotalAsync(ct) + 1;
        var codProiect = $"EW-{numarUrmator:000}";

        var proiect = Proiect.Creeaza(
            clientId: client.Id,
            codProiect: codProiect,
            nume: request.Nume,
            valoare: request.Valoare,
            subtip: request.Subtip,
            tipPrincipal: request.TipPrincipal,
            descriere: request.Descriere,
            termenLimita: request.TermenLimita,
            status: request.Status,
            adresaMontaj: request.AdresaMontaj);

        await _proiectRepository.AdaugaAsync(proiect, ct);
        await _unitOfWork.SalveazaAsync(ct);

        return proiect.Id;
    }

    public async Task<IReadOnlyList<ProiectDto>> ObtineListaProiecteAsync(CancellationToken ct = default)
    {
        var proiecte = await _proiectRepository.ObtineToateAsync(ct);
        var clienti = await _clientRepository.ObtineTotiAsync(ct);
        var clientiDupaId = clienti.ToDictionary(c => c.Id);

        return proiecte.Select(p =>
        {
            clientiDupaId.TryGetValue(p.ClientId, out var client);
            return MapeazaLaDto(p, client);
        }).ToList();
    }

    public async Task<IReadOnlyList<ProiectDto>> ObtineProiecteDupaClientAsync(Guid clientId, CancellationToken ct = default)
    {
        var client = await _clientRepository.ObtineDupaIdAsync(clientId, ct);
        var proiecte = await _proiectRepository.ObtineDupaClientAsync(clientId, ct);

        return proiecte.Select(p => MapeazaLaDto(p, client)).ToList();
    }

    public async Task<ProiectDto?> ObtineProiectDupaIdAsync(Guid id, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(id, ct);
        if (proiect is null) return null;

        var client = await _clientRepository.ObtineDupaIdAsync(proiect.ClientId, ct);
        return MapeazaLaDto(proiect, client);
    }

    public async Task AdaugaNotaAsync(Guid proiectId, string nota, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(proiectId, ct)
            ?? throw new InvalidOperationException("Proiectul nu a fost găsit.");

        proiect.AdaugaNota(nota);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task ActualizeazaProiectAsync(ActualizeazaProiectRequest request, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(request.Id, ct)
            ?? throw new InvalidOperationException("Proiectul nu a fost găsit.");

        proiect.Actualizeaza(
            nume: request.Nume,
            valoare: request.Valoare,
            subtip: request.Subtip,
            tipPrincipal: request.TipPrincipal,
            descriere: request.Descriere,
            termenLimita: request.TermenLimita,
            adresaMontaj: request.AdresaMontaj,
            bugetAlocat: request.BugetAlocat,
            procentManopera: request.ProcentManopera);

        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task SchimbaStatusAsync(Guid proiectId, StatusProiect statusNou, string? observatie = null, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(proiectId, ct)
            ?? throw new InvalidOperationException("Proiectul nu a fost găsit.");

        var statusVechi = proiect.Status;
        if (statusVechi == statusNou) return;

        proiect.SchimbaStatus(statusNou);

        var intrareIstoric = IstoricStatusProiect.Creeaza(proiectId, statusVechi, statusNou, observatie, utilizator: null);
        await _istoricStatusRepository.AdaugaAsync(intrareIstoric, ct);

        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<IReadOnlyList<IstoricStatusDto>> ObtineIstoricStatusAsync(Guid proiectId, CancellationToken ct = default)
    {
        var istoric = await _istoricStatusRepository.ObtineDupaProiectAsync(proiectId, ct);

        return istoric.Select(i => new IstoricStatusDto(
            i.Id,
            i.StatusVechi,
            i.StatusNou,
            i.Observatie,
            i.Utilizator,
            i.DataSchimbare)).ToList();
    }

    public async Task<Guid> AdaugaSpatiuAsync(AdaugaSpatiuRequest request, CancellationToken ct = default)
    {
        var spatiu = SpatiuProiect.Creeaza(
            proiectId: request.ProiectId,
            nume: request.Nume,
            suprafata: request.Suprafata,
            inaltimeMm: request.InaltimeMm,
            orientare: request.Orientare,
            tipSpatiu: request.TipSpatiu,
            starePereti: request.StarePereti,
            starePardoseala: request.StarePardoseala);

        await _spatiuRepository.AdaugaAsync(spatiu, ct);
        await _unitOfWork.SalveazaAsync(ct);

        return spatiu.Id;
    }

    public async Task<IReadOnlyList<SpatiuProiectDto>> ObtineSpatiiDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        var spatii = await _spatiuRepository.ObtineDupaProiectAsync(proiectId, ct);

        return spatii.Select(s => new SpatiuProiectDto(
            s.Id,
            s.Nume,
            s.Suprafata,
            s.InaltimeMm,
            s.Orientare,
            s.TipSpatiu,
            s.StarePereti,
            s.StarePardoseala)).ToList();
    }

    public async Task StergeSpatiuAsync(Guid spatiuId, CancellationToken ct = default)
    {
        var spatiu = await _spatiuRepository.ObtineDupaIdAsync(spatiuId, ct);
        if (spatiu is null) return;

        await _spatiuRepository.StergeAsync(spatiu, ct);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task ActualizeazaObservatiiClientAsync(Guid proiectId, string? observatiiClient, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(proiectId, ct)
            ?? throw new InvalidOperationException("Proiectul nu a fost găsit.");

        proiect.ActualizeazaObservatiiClient(observatiiClient);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task ActualizeazaDateMontajAsync(ActualizeazaDateMontajRequest request, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(request.ProiectId, ct)
            ?? throw new InvalidOperationException("Proiectul nu a fost găsit.");

        proiect.ActualizeazaDateMontaj(
            etaj: request.Etaj,
            areLift: request.AreLift,
            accesAuto: request.AccesAuto,
            locParcare: request.LocParcare,
            persoanaContactMontaj: request.PersoanaContactMontaj,
            intervalOrarPreferat: request.IntervalOrarPreferat,
            observatiiTransport: request.ObservatiiTransport);

        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<Guid> AdaugaNotaInternaAsync(AdaugaNotaInternaRequest request, CancellationToken ct = default)
    {
        var nota = NotaInternaProiect.Creeaza(request.ProiectId, request.Text, request.Autor);
        await _notaInternaRepository.AdaugaAsync(nota, ct);
        await _unitOfWork.SalveazaAsync(ct);
        return nota.Id;
    }

    public async Task ActualizeazaNotaInternaAsync(ActualizeazaNotaInternaRequest request, CancellationToken ct = default)
    {
        var nota = await _notaInternaRepository.ObtineDupaIdAsync(request.Id, ct)
            ?? throw new InvalidOperationException("Nota nu a fost găsită.");

        nota.Actualizeaza(request.Text);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task ComutaFixareNotaAsync(Guid notaId, CancellationToken ct = default)
    {
        var nota = await _notaInternaRepository.ObtineDupaIdAsync(notaId, ct)
            ?? throw new InvalidOperationException("Nota nu a fost găsită.");

        nota.ComutaFixare();
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task ComutaRezolvareNotaAsync(Guid notaId, CancellationToken ct = default)
    {
        var nota = await _notaInternaRepository.ObtineDupaIdAsync(notaId, ct)
            ?? throw new InvalidOperationException("Nota nu a fost găsită.");

        nota.ComutaRezolvare();
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<IReadOnlyList<NotaInternaDto>> ObtineNoteInterneDupaProiectAsync(Guid proiectId, CancellationToken ct = default)
    {
        var note = await _notaInternaRepository.ObtineDupaProiectAsync(proiectId, ct);

        return note.Select(n => new NotaInternaDto(
            n.Id,
            n.Text,
            n.Autor,
            n.EsteFixata,
            n.EsteRezolvata,
            n.DataCreare,
            n.DataModificare)).ToList();
    }

    public async Task SeteazaUrmatorulPasAsync(Guid proiectId, string? descriere, DateTime? termen, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(proiectId, ct)
            ?? throw new InvalidOperationException("Proiectul nu a fost găsit.");

        proiect.SeteazaUrmatorulPas(descriere, termen);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task FinalizeazaUrmatorulPasAsync(Guid proiectId, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(proiectId, ct)
            ?? throw new InvalidOperationException("Proiectul nu a fost găsit.");

        proiect.FinalizeazaUrmatorulPas();
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<IReadOnlyList<ActivitateDto>> ObtineActivitateRecentaAsync(Guid proiectId, int numarMaxim = int.MaxValue, CancellationToken ct = default)
    {
        var istoricStatus = await _istoricStatusRepository.ObtineDupaProiectAsync(proiectId, ct);
        var note = await _notaInternaRepository.ObtineDupaProiectAsync(proiectId, ct);

        var activitati = new List<ActivitateDto>();

        activitati.AddRange(istoricStatus.Select(i => new ActivitateDto(
            TipActivitate.SchimbareStatus,
            $"Status schimbat în {NumeStatusPentruActivitate(i.StatusNou)}",
            i.DataSchimbare,
            i.Utilizator)));

        activitati.AddRange(note.Select(n => new ActivitateDto(
            n.DataModificare.HasValue ? TipActivitate.NotaModificata : TipActivitate.NotaAdaugata,
            n.DataModificare.HasValue ? "Notă internă modificată" : "Notă internă adăugată",
            n.DataModificare ?? n.DataCreare,
            n.Autor)));

        return activitati
            .OrderByDescending(a => a.Data)
            .Take(numarMaxim)
            .ToList();
    }

    public async Task ActualizeazaSetariOfertaAsync(Guid proiectId, decimal? avansProcent, int? termenExecutieZile, int? valabilitateOfertaZile, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(proiectId, ct)
            ?? throw new InvalidOperationException("Proiectul nu a fost găsit.");

        proiect.ActualizeazaSetariOferta(avansProcent, termenExecutieZile, valabilitateOfertaZile);
        await _unitOfWork.SalveazaAsync(ct);
    }

    public async Task<IReadOnlyList<AlertaDto>> ObtineAlerteAsync(Guid proiectId, CancellationToken ct = default)
    {
        var proiect = await _proiectRepository.ObtineDupaIdAsync(proiectId, ct);
        var alerte = new List<AlertaDto>();

        if (proiect is not null && proiect.TermenLimita.HasValue && proiect.Status != StatusProiect.Finalizat)
        {
            var zile = (proiect.TermenLimita.Value.Date - DateTime.Today).Days;

            if (zile < 0)
            {
                alerte.Add(new AlertaDto($"Termenul limită a fost depășit cu {-zile} zile.", Severitate.Critica));
            }
            else if (zile <= 7)
            {
                alerte.Add(new AlertaDto($"Termenul limită este în {zile} zile.", Severitate.Atentionare));
            }
        }

        return alerte;
    }

    private static string NumeStatusPentruActivitate(StatusProiect status) => status switch
    {
        StatusProiect.OfertaTrimisa => "Ofertă trimisă",
        StatusProiect.Masuratori => "Măsurători",
        StatusProiect.Proiectare => "Proiectare",
        StatusProiect.InProductie => "În producție",
        StatusProiect.LaMontaj => "La montaj",
        StatusProiect.Finalizat => "Finalizat",
        _ => status.ToString()
    };

    private static ProiectDto MapeazaLaDto(Proiect proiect, Client? client) => new(
        proiect.Id,
        proiect.CodProiect,
        proiect.Nume,
        proiect.Subtip,
        proiect.TipPrincipal,
        proiect.Descriere,
        proiect.ClientId,
        client?.Nume ?? "(client șters)",
        client?.Telefon ?? "-",
        client?.Email,
        client?.Localitate ?? "-",
        proiect.Status,
        proiect.Valoare,
        proiect.TermenLimita,
        proiect.Progres,
        proiect.Note,
        proiect.AdresaMontaj,
        proiect.ObservatiiClient,
        proiect.Etaj,
        proiect.AreLift,
        proiect.AccesAuto,
        proiect.LocParcare,
        proiect.PersoanaContactMontaj,
        proiect.IntervalOrarPreferat,
        proiect.ObservatiiTransport,
        proiect.DescriereUrmatorulPas,
        proiect.TermenUrmatorulPas,
        proiect.BugetAlocat,
        proiect.ProcentManopera,
        proiect.AvansProcent,
        proiect.TermenExecutieZile,
        proiect.ValabilitateOfertaZile);
}
