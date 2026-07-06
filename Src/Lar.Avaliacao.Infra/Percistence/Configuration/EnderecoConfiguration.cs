using Lar.Avaliacao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Infra.Percistence.Configuration
{
    public class EnderecoConfiguration : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("Enderecos");
            builder.HasKey(e => e.Id);

            builder.Property(e => e.PessoaId)
                .IsRequired();

            builder.Property(e => e.Rua)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(e => e.Numero)
                .HasMaxLength(10)
                .IsRequired();

            builder.Property(e => e.Complemento)
                .HasMaxLength(250);

            builder.Property(e => e.Referencia)
                .HasMaxLength(250);

            builder.Property(e => e.Cidade)
                .HasMaxLength(150)
                .IsRequired();

            builder.Property(e => e.Estado)
                .HasMaxLength(2)
                .IsRequired();

            builder.HasOne<Pessoa>()
                .WithMany()
                .HasForeignKey(e => e.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(e => e.PessoaId);
        }
    }
}
