namespace EvaWoods.Domain.Oferte;

public interface ILinieOfertaRepository
{
    Task AdaugaAsync(LinieOferta linie, CancellationToken ct = default);
    Task<IReadOnlyList<LinieOferta>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task<LinieOferta?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default);
    Task StergeAsync(LinieOferta linie, CancellationToken ct = default);
    Task StergeToateAutomateAsync(Guid proiectId, CancellationToken ct = default);
}