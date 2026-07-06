using Lar.Avaliacao.Domain.Entities;


namespace Lar.Avaliacao.Domain.Interfaces
{
    public interface ITelefoneRepository
    {
        Task<Telefone?> ObterPorIdAsync(Guid id, CancellationToken ct = default);

        Task<IEnumerable<Telefone>> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken ct = default);

        Task AdicionarAsync(Telefone telefone, CancellationToken ct = default);

        void Remover(Telefone telefone);
    }
}
