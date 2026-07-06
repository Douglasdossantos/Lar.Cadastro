using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Lar.Avaliacao.Infra.Percistence.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly AppDbContext _context;

        public PessoaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(Pessoa pessoa, CancellationToken ct = default)
        {
            await _context.Pessoas.AddAsync(pessoa);
        }

        public void Atualizar(Pessoa pessoa)
        {
            if (_context.Entry(pessoa).State == EntityState.Detached)
            {
                _context.Pessoas.Update(pessoa);
            }
        }

        public async Task<bool> ExisteAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Pessoas.AsNoTracking().AnyAsync(p => p.Id == id, ct);
        }

        public async Task<bool> ExisteComCpfAsync(string cpf, CancellationToken ct = default)
        {
            var apenasDigitos = new string(cpf.Where(char.IsDigit).ToArray());

            return await _context.Pessoas
                .AsNoTracking()
                .AnyAsync(p => p.Cpf.Numero == apenasDigitos, ct);
        }

        public async Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Pessoas.FirstOrDefaultAsync(p => p.Id == id, ct);
        }

        public async Task<IEnumerable<Pessoa>> ObterTodosAsync(CancellationToken ct = default)
        {
            return await _context.Pessoas
            .AsNoTracking()
            .ToListAsync(ct);
        }

        public void Remover(Pessoa pessoa)
        {
            _context.Pessoas.Remove(pessoa);
        }
    }
}
