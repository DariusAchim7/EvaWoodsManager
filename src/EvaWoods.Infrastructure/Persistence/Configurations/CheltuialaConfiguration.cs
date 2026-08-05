using EvaWoods.Domain.Cheltuieli;
using EvaWoods.Domain.Proiecte;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class CheltuialaConfiguration : IEntityTypeConfiguration<Cheltuiala>
{
    public void Configure(EntityTypeBuilder<Cheltuiala> builder)
    {
        builder.ToTable("Cheltuieli");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Descriere).HasMaxLength(300).IsRequired();
        builder.Property(c => c.Categorie).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.Subcategorie).HasMaxLength(150);
        builder.Property(c => c.NumarFactura).HasMaxLength(50);
        builder.Property(c => c.ValoareFaraTva).HasColumnType("numeric(12,2)");
        builder.Property(c => c.ProcentTva).HasColumnType("numeric(5,2)");
        builder.Property(c => c.Tva).HasColumnType("numeric(12,2)");
        builder.Property(c => c.Total).HasColumnType("numeric(12,2)");
        builder.Property(c => c.MetodaPlata).HasConversion<string>().HasMaxLength(30);
        builder.Property(c => c.StatusPlata).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Observatii).HasMaxLength(500);
        builder.Property(c => c.Data).HasColumnType("date");

        builder.HasOne<Proiect>()
            .WithMany()
            .HasForeignKey(c => c.ProiectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Furnizor>()
            .WithMany()
            .HasForeignKey(c => c.FurnizorId)
            .OnDelete(DeleteBehavior.SetNull);
    }
}