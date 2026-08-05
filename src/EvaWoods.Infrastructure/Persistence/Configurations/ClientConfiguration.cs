using EvaWoods.Domain.Clienti;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
    public void Configure(EntityTypeBuilder<Client> builder)
    {
        builder.ToTable("Clienti");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Nume).HasMaxLength(200).IsRequired();
        builder.Property(c => c.Telefon).HasMaxLength(30).IsRequired();
        builder.Property(c => c.Email).HasMaxLength(200);
        builder.Property(c => c.Localitate).HasMaxLength(100).IsRequired();
        builder.Property(c => c.Judet).HasMaxLength(100);
        builder.Property(c => c.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(c => c.Note).HasMaxLength(2000);

        builder.HasIndex(c => c.Nume);
    }
}