using EvaWoods.Domain.Clienti;
using EvaWoods.Domain.Proiecte;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EvaWoods.Infrastructure.Persistence.Configurations;

public class ProiectConfiguration : IEntityTypeConfiguration<Proiect>
{
    public void Configure(EntityTypeBuilder<Proiect> builder)
    {
        builder.ToTable("Proiecte");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.CodProiect).HasMaxLength(20).IsRequired();
        builder.HasIndex(p => p.CodProiect).IsUnique();

        builder.Property(p => p.Nume).HasMaxLength(200).IsRequired();
        builder.Property(p => p.Subtip).HasMaxLength(150).HasColumnName("TipMobilier");
        builder.Property(p => p.TipPrincipal).HasConversion<string>().HasMaxLength(30);
        builder.Property(p => p.Descriere).HasMaxLength(2000);
        builder.Property(p => p.Status).HasConversion<string>().HasMaxLength(20);
        builder.Property(p => p.Valoare).HasColumnType("numeric(12,2)");
        builder.Property(p => p.TermenLimita).HasColumnType("date");
        builder.Property(p => p.AdresaMontaj).HasMaxLength(300);
        builder.Property(p => p.Note).HasMaxLength(2000);
        builder.Property(p => p.ObservatiiClient).HasMaxLength(2000);
        builder.Property(p => p.Etaj).HasMaxLength(30);
        builder.Property(p => p.AccesAuto).HasMaxLength(300);
        builder.Property(p => p.LocParcare).HasMaxLength(300);
        builder.Property(p => p.PersoanaContactMontaj).HasMaxLength(150);
        builder.Property(p => p.IntervalOrarPreferat).HasMaxLength(100);
        builder.Property(p => p.ObservatiiTransport).HasMaxLength(1000);
        builder.Property(p => p.DescriereUrmatorulPas).HasMaxLength(500);
        builder.Property(p => p.TermenUrmatorulPas).HasColumnType("date");
        builder.Property(p => p.BugetAlocat).HasColumnType("numeric(12,2)");
        builder.Property(p => p.ProcentManopera).HasColumnType("numeric(5,2)");
        builder.Property(p => p.AvansProcent).HasColumnType("numeric(5,2)");

        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(p => p.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}