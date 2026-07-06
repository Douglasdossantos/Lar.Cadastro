using Lar.Avaliacao.Domain.Entities;

namespace Lar.Avaliacao.Domain.Interfaces
{
    public interface IUsuarioRepository
    {
        Task<Usuario?> ObterPorEmailAsync(string email, CancellationToken ct = default);

        Task<bool> ExisteComEmailAsync(string email, CancellationToken ct = default);

        Task AdicionarAsync(Usuario usuario, CancellationToken ct = default);
    }
}
