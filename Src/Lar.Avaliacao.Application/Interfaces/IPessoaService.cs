using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Application.Interfaces
{
    public interface IPessoaService
    {
        Task<IEnumerable<PessoaDto>> ObterTodosAsync(CancellationToken ct = default);

        Task<PessoaDto> ObterPorIdAsync(Guid id, CancellationToken ct = default);

        Task<PessoaDto> CriarAsync(CriarPessoaRequest request, CancellationToken ct = default);

        Task AtualizarAsync(Guid id, AtualizarPessoaRequest request, CancellationToken ct = default);

        Task RemoverAsync(Guid id, CancellationToken ct = default);
    }
}
