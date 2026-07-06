using Lar.Avaliacao.Domain.Entities;

namespace Lar.Avaliacao.Domain.Interfaces
{
    public interface IPessoaRepository
    {
        Task<Pessoa?> ObterPorIdAsync(Guid id, CancellationToken ct = default);

        Task<bool> ExisteAsync(Guid id, CancellationToken ct = default);

        Task<IEnumerable<Pessoa>> ObterTodosAsync(CancellationToken ct = default);

        Task<bool> ExisteComCpfAsync(string cpf, CancellationToken ct = default);

        Task AdicionarAsync(Pessoa pessoa, CancellationToken ct = default);

        void Atualizar(Pessoa pessoa);

        void Remover(Pessoa pessoa);
    }
}
