using EvaWoods.Domain.Proiecte;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class IstoricStatusProiectConfiguration : IEntityTypeConfiguration<IstoricStatusProiect>
{
    public void Configure(EntityTypeBuilder<IstoricStatusProiect> builder)
    {
        builder.ToTable("IstoricStatusProiect");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.StatusVechi).HasConversion<string>().HasMaxLength(20);
        builder.Property(i => i.StatusNou).HasConversion<string>().HasMaxLength(20);
        builder.Property(i => i.Observatie).HasMaxLength(1000);
        builder.Property(i => i.Utilizator).HasMaxLength(150);

        builder.HasOne<Proiect>()
            .WithMany()
            .HasForeignKey(i => i.ProiectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}