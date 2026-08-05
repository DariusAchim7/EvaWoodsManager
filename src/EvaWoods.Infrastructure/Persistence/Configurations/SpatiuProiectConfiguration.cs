using EvaWoods.Domain.Proiecte;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class SpatiuProiectConfiguration : IEntityTypeConfiguration<SpatiuProiect>
{
    public void Configure(EntityTypeBuilder<SpatiuProiect> builder)
    {
        builder.ToTable("SpatiiProiect");
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Nume).HasMaxLength(150).IsRequired();
        builder.Property(s => s.Suprafata).HasColumnType("numeric(8,2)");
        builder.Property(s => s.Orientare).HasMaxLength(50);
        builder.Property(s => s.TipSpatiu).HasMaxLength(100);
        builder.Property(s => s.StarePereti).HasMaxLength(500);
        builder.Property(s => s.StarePardoseala).HasMaxLength(500);

        builder.HasOne<Proiect>()
            .WithMany()
            .HasForeignKey(s => s.ProiectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}