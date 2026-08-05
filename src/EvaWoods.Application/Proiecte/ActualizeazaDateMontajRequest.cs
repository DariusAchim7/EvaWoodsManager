namespace EvaWoods.Application.Proiecte;

public record ActualizeazaDateMontajRequest(
    Guid ProiectId,
    string? Etaj,
    bool? AreLift,
    string? AccesAuto,
    string? LocParcare,
    string? PersoanaContactMontaj,
    string? IntervalOrarPreferat,
    string? ObservatiiTransport);