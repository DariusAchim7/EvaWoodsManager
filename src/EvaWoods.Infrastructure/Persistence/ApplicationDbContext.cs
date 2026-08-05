using EvaWoods.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EvaWoods.Domain.Clienti;
using EvaWoods.Domain.Proiecte;
using EvaWoods.Domain.Cheltuieli;
using EvaWoods.Domain.Produse;
using EvaWoods.Domain.Piese;
using EvaWoods.Domain.Materiale;
using EvaWoods.Domain.Oferte;

namespace EvaWoods.Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    public DbSet<Client> Clienti => Set<Client>();
    public DbSet<Proiect> Proiecte => Set<Proiect>();
    public DbSet<IstoricStatusProiect> IstoricStatusProiect => Set<IstoricStatusProiect>();
    public DbSet<SpatiuProiect> SpatiiProiect => Set<SpatiuProiect>();
    public DbSet<NotaInternaProiect> NoteInterneProiect => Set<NotaInternaProiect>();
    public DbSet<Cheltuiala> Cheltuieli => Set<Cheltuiala>();
    public DbSet<Furnizor> Furnizori => Set<Furnizor>();
    public DbSet<Produs> Produse => Set<Produs>();
    public DbSet<CorpProiect> CorpuriProiect => Set<CorpProiect>();
    public DbSet<Piesa> Piese => Set<Piesa>();
    public DbSet<CalculMaterial> CalculeMateriale => Set<CalculMaterial>();
    public DbSet<LinieOferta> LiniiOferta => Set<LinieOferta>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new Configurations.ClientConfiguration());
        builder.ApplyConfiguration(new Configurations.ProiectConfiguration());
        builder.ApplyConfiguration(new Configurations.IstoricStatusProiectConfiguration());
        builder.ApplyConfiguration(new Configurations.SpatiuProiectConfiguration());
        builder.ApplyConfiguration(new Configurations.NotaInternaProiectConfiguration());
        builder.ApplyConfiguration(new Configurations.CheltuialaConfiguration());
        builder.ApplyConfiguration(new Configurations.FurnizorConfiguration());
        builder.ApplyConfiguration(new Configurations.ProdusConfiguration());
        builder.ApplyConfiguration(new Configurations.CorpProiectConfiguration());
        builder.ApplyConfiguration(new Configurations.PiesaConfiguration());
        builder.ApplyConfiguration(new Configurations.CalculMaterialConfiguration());
        builder.ApplyConfiguration(new Configurations.LinieOfertaConfiguration());
    }   
}