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
    public class EnderecoController : ControllerBase
    {
        private readonly IEnderecoService _enderecoService;

        public EnderecoController(IEnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        [HttpGet("ObterPorIdPessoa")]
        [ProducesResponseType(typeof(PessoaComEnderecosDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PessoaComEnderecosDto>> ObterPorPessoa(Guid pessoaId, CancellationToken ct)
        {
            var resultado = await _enderecoService.ObterPorPessoaIdAsync(pessoaId, ct);
            return Ok(resultado);
        }

        [HttpPost("Adicionar")]
        [ProducesResponseType(typeof(EnderecoDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EnderecoDto>> Adicionar(
        Guid pessoaId, AdicionarEnderecoRequest request, CancellationToken ct)
        {
            var endereco = await _enderecoService.AdicionarAsync(pessoaId, request, ct);
            return CreatedAtAction(nameof(ObterPorPessoa), new { pessoaId }, endereco);
        }

        [HttpDelete("Excluir/{enderecoId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(Guid pessoaId, Guid enderecoId, CancellationToken ct)
        {
            await _enderecoService.RemoverAsync(pessoaId, enderecoId, ct);
            return NoContent();
        }
    }
}
