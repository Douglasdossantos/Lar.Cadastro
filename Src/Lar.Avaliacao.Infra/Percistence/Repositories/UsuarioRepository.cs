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
    public class UsuarioRepository : IUsuarioRepository
    {
         private readonly AppDbContext _context;
        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default)
        {
            var emailNormalizado = email.Trim().ToLowerInvariant();
            return await _context.Usuario.FirstOrDefaultAsync(u => u.Email == emailNormalizado, ct);
        }

        public async Task<bool> ExisteComEmailAsync(string email, CancellationToken ct = default)
        {
            var emailNormalizado = email.Trim().ToLowerInvariant();
            return await _context.Usuario.AsNoTracking().AnyAsync(u => u.Email == emailNormalizado, ct);
        }

        public async Task AdicionarAsync(Usuario usuario, CancellationToken ct = default)
        {
            await _context.Usuario.AddAsync(usuario, ct);
        }
    }
}
