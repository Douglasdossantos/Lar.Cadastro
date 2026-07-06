using Lar.Avaliacao.Domain.Entities;

namespace Lar.Avaliacao.Domain.Interfaces
{
    public interface IEnderecoRepository
    {
        Task<Endereco?> ObterPorIdAsync(Guid id, CancellationToken ct = default);

        Task<IEnumerable<Endereco>> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken ct = default);

        Task AdicionarAsync(Endereco endereco, CancellationToken ct = default);

        void Remover(Endereco endereco);
    }
}
