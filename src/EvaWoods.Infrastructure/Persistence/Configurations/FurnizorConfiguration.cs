using EvaWoods.Domain.Cheltuieli;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class FurnizorConfiguration : IEntityTypeConfiguration<Furnizor>
{
    public void Configure(EntityTypeBuilder<Furnizor> builder)
    {
        builder.ToTable("Furnizori");
        builder.HasKey(f => f.Id);
        builder.Property(f => f.Nume).HasMaxLength(200).IsRequired();
    }
}