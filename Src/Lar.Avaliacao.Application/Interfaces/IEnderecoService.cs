using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Application.Interfaces
{
    public interface IEnderecoService
    {
        Task<PessoaComEnderecosDto> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken ct = default);

        Task<EnderecoDto> AdicionarAsync(Guid pessoaId, AdicionarEnderecoRequest request, CancellationToken ct = default);

        Task RemoverAsync(Guid pessoaId, Guid enderecoId, CancellationToken ct = default);
    }
}
