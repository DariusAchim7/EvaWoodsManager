namespace EvaWoods.Domain.Cheltuieli;

public class Furnizor
{
    public Guid Id { get; private set; }
    public string Nume { get; private set; } = string.Empty;

    private Furnizor() { }

    public static Furnizor Creeaza(string nume)
    {
        if (string.IsNullOrWhiteSpace(nume))
            throw new ArgumentException("Numele furnizorului este obligatoriu.", nameof(nume));

        return new Furnizor { Id = Guid.NewGuid(), Nume = nume.Trim() };
    }
}