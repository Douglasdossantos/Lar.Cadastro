using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Requests;

namespace Lar.Avaliacao.Application.Interfaces
{
    public interface ITelefoneService
    {
        Task<PessoaComTelefonesDto> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken ct = default);

        Task<TelefoneDto> AdicionarAsync(Guid pessoaId, AdicionarTelefoneRequest request, CancellationToken ct = default);

        Task RemoverAsync(Guid pessoaId, Guid telefoneId, CancellationToken ct = default);
    }
}
