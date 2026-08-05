using EvaWoods.Domain.Proiecte;

namespace EvaWoods.Application.Proiecte;


public interface IProiectService
{
    Task<Guid> CreeazaProiectAsync(CreeazaProiectRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<ProiectDto>> ObtineListaProiecteAsync(CancellationToken ct = default);
    Task<IReadOnlyList<ProiectDto>> ObtineProiecteDupaClientAsync(Guid clientId, CancellationToken ct = default);
    Task<ProiectDto?> ObtineProiectDupaIdAsync(Guid id, CancellationToken ct = default);
    Task AdaugaNotaAsync(Guid proiectId, string nota, CancellationToken ct = default);
    Task ActualizeazaProiectAsync(ActualizeazaProiectRequest request, CancellationToken ct = default);
    Task ActualizeazaObservatiiClientAsync(Guid proiectId, string? observatiiClient, CancellationToken ct = default);
    Task ActualizeazaDateMontajAsync(ActualizeazaDateMontajRequest request, CancellationToken ct = default);
    Task<Guid> AdaugaSpatiuAsync(AdaugaSpatiuRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<SpatiuProiectDto>> ObtineSpatiiDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task StergeSpatiuAsync(Guid spatiuId, CancellationToken ct = default);
    Task SchimbaStatusAsync(Guid proiectId, StatusProiect statusNou, string? observatie = null, CancellationToken ct = default);
    Task<IReadOnlyList<IstoricStatusDto>> ObtineIstoricStatusAsync(Guid proiectId, CancellationToken ct = default);

    Task<Guid> AdaugaNotaInternaAsync(AdaugaNotaInternaRequest request, CancellationToken ct = default);
    Task ActualizeazaNotaInternaAsync(ActualizeazaNotaInternaRequest request, CancellationToken ct = default);
    Task ComutaFixareNotaAsync(Guid notaId, CancellationToken ct = default);
    Task ComutaRezolvareNotaAsync(Guid notaId, CancellationToken ct = default);
    Task<IReadOnlyList<NotaInternaDto>> ObtineNoteInterneDupaProiectAsync(Guid proiectId, CancellationToken ct = default);

    Task SeteazaUrmatorulPasAsync(Guid proiectId, string? descriere, DateTime? termen, CancellationToken ct = default);
    Task FinalizeazaUrmatorulPasAsync(Guid proiectId, CancellationToken ct = default);

    Task<IReadOnlyList<ActivitateDto>> ObtineActivitateRecentaAsync(Guid proiectId, int numarMaxim = int.MaxValue, CancellationToken ct = default);
    Task<IReadOnlyList<AlertaDto>> ObtineAlerteAsync(Guid proiectId, CancellationToken ct = default);
    Task ActualizeazaSetariOfertaAsync(Guid proiectId, decimal? avansProcent, int? termenExecutieZile, int? valabilitateOfertaZile, CancellationToken ct = default);
}