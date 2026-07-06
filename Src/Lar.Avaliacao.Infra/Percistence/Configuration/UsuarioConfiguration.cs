using Lar.Avaliacao.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Lar.Avaliacao.Infra.Percistence.Configuration
{
    public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("Usuarios");
            builder.HasKey(u => u.Id);

            builder.Property(u => u.Nome)
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(u => u.Email)
                .HasMaxLength(250)
                .IsRequired();

            builder.HasIndex(u => u.Email).IsUnique();

            builder.Property(u => u.SenhaHash)
                .IsRequired();

            builder.Property(u => u.Ativo)
                .IsRequired();
        }
    }
}
