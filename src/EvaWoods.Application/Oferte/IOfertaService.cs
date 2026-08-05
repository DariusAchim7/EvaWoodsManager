namespace EvaWoods.Application.Oferte;

public interface IOfertaService
{
    Task<IReadOnlyList<LinieOfertaDto>> ObtineLiniiAsync(Guid proiectId, CancellationToken ct = default);
    Task AdaugaLinieAsync(AdaugaLinieOfertaRequest request, CancellationToken ct = default);
    Task ActualizeazaCantitateAsync(Guid linieId, decimal cantitate, CancellationToken ct = default);
    Task ComutaVizibilClientAsync(Guid linieId, CancellationToken ct = default);
    Task StergeLinieAsync(Guid linieId, CancellationToken ct = default);
    Task SincronizeazaLiniiAutomateAsync(Guid proiectId, CancellationToken ct = default);
    Task<RezumatOfertaDto> ObtineRezumatAsync(Guid proiectId, CancellationToken ct = default);
}