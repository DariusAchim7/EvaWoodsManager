using EvaWoods.Domain.Oferte;
using EvaWoods.Domain.Produse;
using EvaWoods.Domain.Proiecte;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class LinieOfertaConfiguration : IEntityTypeConfiguration<LinieOferta>
{
    public void Configure(EntityTypeBuilder<LinieOferta> builder)
    {
        builder.ToTable("LiniiOferta");
        builder.HasKey(l => l.Id);

        builder.Property(l => l.DescriereSnapshot).HasMaxLength(200).IsRequired();
        builder.Property(l => l.CategorieSnapshot).HasConversion<string>().HasMaxLength(30);
        builder.Property(l => l.UMSnapshot).HasConversion<string>().HasMaxLength(30);
        builder.Property(l => l.CantitateCalculata).HasColumnType("numeric(12,3)");
        builder.Property(l => l.CantitateOfertata).HasColumnType("numeric(12,3)");
        builder.Property(l => l.PretCostSnapshot).HasColumnType("numeric(12,2)");
        builder.Property(l => l.PretVanzareSnapshot).HasColumnType("numeric(12,2)");
        builder.Property(l => l.ProcentTva).HasColumnType("numeric(5,2)");
        builder.Property(l => l.Nota).HasMaxLength(500);

        builder.HasOne<Proiect>()
            .WithMany()
            .HasForeignKey(l => l.ProiectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Produs>()
            .WithMany()
            .HasForeignKey(l => l.ProdusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}