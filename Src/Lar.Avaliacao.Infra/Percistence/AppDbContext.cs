using Lar.Avaliacao.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lar.Avaliacao.Infra.Percistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        public DbSet<Pessoa> Pessoas => Set<Pessoa>();
        public DbSet<Telefone> Telefone => Set<Telefone>();

        public DbSet<Endereco> Endereco => Set<Endereco>();

        public DbSet<Usuario> Usuario => Set<Usuario>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
