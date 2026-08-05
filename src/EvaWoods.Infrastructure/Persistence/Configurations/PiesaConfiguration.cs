using EvaWoods.Domain.Piese;
using EvaWoods.Domain.Produse;
using EvaWoods.Domain.Proiecte;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class PiesaConfiguration : IEntityTypeConfiguration<Piesa>
{
    public void Configure(EntityTypeBuilder<Piesa> builder)
    {
        builder.ToTable("Piese");
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Nume).HasMaxLength(150);
        builder.Property(p => p.LungimeMm).HasColumnType("numeric(10,2)");
        builder.Property(p => p.LatimeMm).HasColumnType("numeric(10,2)");
        builder.Property(p => p.GrosimeMm).HasColumnType("numeric(6,2)");
        builder.Property(p => p.DirectieFibra).HasConversion<string>().HasMaxLength(20);
        builder.Property(p => p.Observatii).HasMaxLength(500);

        builder.HasOne<Proiect>()
            .WithMany()
            .HasForeignKey(p => p.ProiectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<CorpProiect>()
            .WithMany()
            .HasForeignKey(p => p.CorpId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Produs>()
            .WithMany()
            .HasForeignKey(p => p.MaterialProdusId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasOne<Produs>()
            .WithMany()
            .HasForeignKey(p => p.MaterialCantProdusId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}