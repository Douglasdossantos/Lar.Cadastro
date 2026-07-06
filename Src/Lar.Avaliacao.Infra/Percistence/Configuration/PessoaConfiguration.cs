using Lar.Avaliacao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lar.Avaliacao.Infra.Percistence.Configuration
{
    public class PessoaConfiguration : IEntityTypeConfiguration<Pessoa>
    {
        public void Configure(EntityTypeBuilder<Pessoa> builder)
        {
            builder.ToTable("Pessoas");
            builder.HasKey(p => p.Id);

            builder.Property(p => p.Nome)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(p => p.DataNascimento)
                .IsRequired();

            builder.Property(p => p.Ativo)
                .IsRequired();

            builder.OwnsOne(p => p.Cpf, cpf =>
            {
                cpf.Property(c => c.Numero)
                    .HasColumnName("Cpf")
                    .HasMaxLength(11)
                    .IsRequired();

                cpf.HasIndex(c => c.Numero).IsUnique();
            });
        }
    }
}
