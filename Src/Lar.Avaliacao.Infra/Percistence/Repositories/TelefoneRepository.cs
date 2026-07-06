using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Infra.Percistence.Repositories
{
    public class TelefoneRepository : ITelefoneRepository
    {
        private readonly AppDbContext _context;

        public TelefoneRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Telefone?> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        {
            return await _context.Telefone.FirstOrDefaultAsync(t => t.Id == id, ct);
        }

        public async Task<IEnumerable<Telefone>> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken ct = default)
        {
            return await _context.Telefone
                .AsNoTracking()
                .Where(t => t.PessoaId == pessoaId)
                .ToListAsync(ct);
        }

        public async Task AdicionarAsync(Telefone telefone, CancellationToken ct = default)
        {
            await _context.Telefone.AddAsync(telefone, ct);
        }

        public void Remover(Telefone telefone)
        {
            _context.Telefone.Remove(telefone);
        }
    }
}
