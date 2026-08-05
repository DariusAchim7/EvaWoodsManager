namespace EvaWoods.Domain.Proiecte;

public interface INotaInternaProiectRepository
{
    Task AdaugaAsync(NotaInternaProiect nota, CancellationToken ct = default);
    Task<IReadOnlyList<NotaInternaProiect>> ObtineDupaProiectAsync(Guid proiectId, CancellationToken ct = default);
    Task<NotaInternaProiect?> ObtineDupaIdAsync(Guid id, CancellationToken ct = default);
}