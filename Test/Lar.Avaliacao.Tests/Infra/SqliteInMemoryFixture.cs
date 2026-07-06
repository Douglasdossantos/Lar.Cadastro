using Lar.Avaliacao.Infra.Percistence;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Lar.Avaliacao.Tests.Infra
{
    public sealed class SqliteInMemoryFixture : IDisposable
    {
        private readonly SqliteConnection _connection;

        public AppDbContext CriarContexto()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlite(_connection)
                .Options;

            return new AppDbContext(options);
        }

        public SqliteInMemoryFixture()
        {
            _connection = new SqliteConnection("Data Source=:memory:");
            _connection.Open();

            using var context = CriarContexto();
            context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _connection.Dispose();
        }
    }
}
