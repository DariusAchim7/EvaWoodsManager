using EvaWoods.Domain.Materiale;
using EvaWoods.Domain.Produse;
using EvaWoods.Domain.Proiecte;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class CalculMaterialConfiguration : IEntityTypeConfiguration<CalculMaterial>
{
    public void Configure(EntityTypeBuilder<CalculMaterial> builder)
    {
        builder.ToTable("CalculeMateriale");
        builder.HasKey(c => c.Id);
        builder.HasIndex(c => new { c.ProiectId, c.MaterialProdusId }).IsUnique();

        builder.Property(c => c.SuprafataPieseM2).HasColumnType("numeric(12,4)");
        builder.Property(c => c.FoiTeoretice).HasColumnType("numeric(10,2)");
        builder.Property(c => c.UtilizareProcent).HasColumnType("numeric(5,2)");
        builder.Property(c => c.PierdereProcent).HasColumnType("numeric(5,2)");
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.Erori).HasMaxLength(2000);

        builder.HasOne<Proiect>()
            .WithMany()
            .HasForeignKey(c => c.ProiectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Produs>()
            .WithMany()
            .HasForeignKey(c => c.MaterialProdusId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}