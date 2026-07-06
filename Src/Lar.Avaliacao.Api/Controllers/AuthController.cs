using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lar.Avaliacao.Api.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAutenticacaoService _autenticacaoService;

        public AuthController(IAutenticacaoService autenticacaoService)
        {
            _autenticacaoService = autenticacaoService;
        }

        [HttpPost("registrar")]
        [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<UsuarioDto>> Registrar(RegistrarUsuarioRequest request, CancellationToken ct)
        {
            var usuario = await _autenticacaoService.RegistrarAsync(request, ct);
            return CreatedAtAction(nameof(Registrar), new { usuario.Id }, usuario);
        }

        [HttpPost("login")]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<LoginResponseDto>> Login(LoginRequest request, CancellationToken ct)
        {
            var resultado = await _autenticacaoService.LoginAsync(request, ct);
            return Ok(resultado);
        }
    }
}
