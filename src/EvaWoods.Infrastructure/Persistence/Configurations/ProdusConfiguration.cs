using EvaWoods.Domain.Cheltuieli;
using EvaWoods.Domain.Produse;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class ProdusConfiguration : IEntityTypeConfiguration<Produs>
{
    public void Configure(EntityTypeBuilder<Produs> builder)
    {
        builder.ToTable("Produse");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Cod).HasMaxLength(20).IsRequired();
        builder.HasIndex(p => p.Cod).IsUnique();

        builder.Property(p => p.Nume).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Categorie).HasConversion<string>().HasMaxLength(30);
        builder.Property(p => p.Subcategorie).HasMaxLength(150);
        builder.Property(p => p.UM).HasConversion<string>().HasMaxLength(30);
        builder.Property(p => p.PretAchizitie).HasColumnType("numeric(12,2)");
        builder.Property(p => p.PretCalcul).HasColumnType("numeric(12,2)");
        builder.Property(p => p.ProcentTva).HasColumnType("numeric(5,2)");
        builder.Property(p => p.Observatii).HasMaxLength(1000);
        builder.Property(p => p.LungimeFoaieMm).HasColumnType("numeric(10,2)");
        builder.Property(p => p.LatimeFoaieMm).HasColumnType("numeric(10,2)");
        builder.Property(p => p.GrosimeMm).HasColumnType("numeric(6,2)");
        builder.Property(p => p.KerfMm).HasColumnType("numeric(5,2)");

        builder.HasOne<Furnizor>()
            .WithMany()
            .HasForeignKey(p => p.FurnizorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Produs>()
            .WithMany()
            .HasForeignKey(p => p.ServiciuAsociatId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}