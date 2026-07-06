using Lar.Avaliacao.Domain.Interfaces;
using Lar.Avaliacao.Infra.Percistence;

namespace Lar.Avaliacao.Infra
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }
        public Task<int> SalvarAlteracoesAsync(CancellationToken ct = default) =>
            _context.SaveChangesAsync(ct);
    }
}
