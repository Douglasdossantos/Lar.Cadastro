using Lar.Avaliacao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lar.Avaliacao.Infra.Percistence.Configuration
{
    public class TelefoneConfiguration : IEntityTypeConfiguration<Telefone>
    {
        public void Configure(EntityTypeBuilder<Telefone> builder)
        {
            builder.ToTable("Telefones");
            builder.HasKey(t => t.Id);

            builder.Property(t => t.PessoaId)
                .IsRequired();

            builder.Property(t => t.Tipo)
                .HasConversion<int>()   
                .IsRequired();

            builder.Property(t => t.Numero)
                .HasMaxLength(11)
                .IsRequired();

            builder.HasOne<Pessoa>()
                .WithMany()
                .HasForeignKey(t => t.PessoaId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(t => t.PessoaId);
        }
    }
}
