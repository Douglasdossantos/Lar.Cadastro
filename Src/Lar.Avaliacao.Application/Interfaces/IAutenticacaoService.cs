using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Application.Interfaces
{
    public interface IAutenticacaoService
    {
        Task<UsuarioDto> RegistrarAsync(RegistrarUsuarioRequest request, CancellationToken ct = default);

        Task<LoginResponseDto> LoginAsync(LoginRequest request, CancellationToken ct = default);
    }
}
