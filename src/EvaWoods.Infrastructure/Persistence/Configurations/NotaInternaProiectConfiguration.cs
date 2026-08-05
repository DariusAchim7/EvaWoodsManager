using EvaWoods.Domain.Proiecte;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class NotaInternaProiectConfiguration : IEntityTypeConfiguration<NotaInternaProiect>
{
    public void Configure(EntityTypeBuilder<NotaInternaProiect> builder)
    {
        builder.ToTable("NoteInterneProiect");
        builder.HasKey(n => n.Id);

        builder.Property(n => n.Text).HasMaxLength(2000).IsRequired();
        builder.Property(n => n.Autor).HasMaxLength(150);

        builder.HasOne<Proiect>()
            .WithMany()
            .HasForeignKey(n => n.ProiectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}