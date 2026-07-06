using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lar.Avaliacao.Infra.Percistence.Repositories
{
    public class EnderecoRepository : IEnderecoRepository
    {
        private readonly AppDbContext _context;

        public EnderecoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Endereco?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Endereco.FirstOrDefaultAsync(e => e.Id == id, ct);
        }

        public async Task<IEnumerable<Endereco>> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken ct = default)
        {
            return await _context.Endereco
                .AsNoTracking()
                .Where(e => e.PessoaId == pessoaId)
                .ToListAsync(ct);
        }

        public async Task AdicionarAsync(Endereco endereco, CancellationToken ct = default)
        {
            await _context.Endereco.AddAsync(endereco, ct);
        }

        public void Remover(Endereco endereco)
        {
            _context.Endereco.Remove(endereco);
        }
    }
}
