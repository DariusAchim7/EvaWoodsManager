using EvaWoods.Domain.Piese;
using EvaWoods.Domain.Proiecte;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class CorpProiectConfiguration : IEntityTypeConfiguration<CorpProiect>
{
    public void Configure(EntityTypeBuilder<CorpProiect> builder)
    {
        builder.ToTable("CorpuriProiect");
        builder.HasKey(c => c.Id);
        builder.Property(c => c.Nume).HasMaxLength(150).IsRequired();

        builder.HasOne<Proiect>()
            .WithMany()
            .HasForeignKey(c => c.ProiectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}