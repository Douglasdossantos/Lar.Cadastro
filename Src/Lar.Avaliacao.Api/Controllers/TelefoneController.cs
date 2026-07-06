using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lar.Avaliacao.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TelefoneController : Controller
    {
        private readonly ITelefoneService _telefoneService;

        public TelefoneController(ITelefoneService telefoneService)
        {
            _telefoneService = telefoneService;
        }

        [HttpGet("ObterPorIdPessoa")]
        [ProducesResponseType(typeof(PessoaComTelefonesDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PessoaComTelefonesDto>> ObterPorPessoa(Guid pessoaId, CancellationToken ct)
        {
            var resultado = await _telefoneService.ObterPorPessoaIdAsync(pessoaId, ct);
            return Ok(resultado);
        }

        [HttpPost("Adicionar")]
        [ProducesResponseType(typeof(TelefoneDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<TelefoneDto>> Adicionar(
        Guid pessoaId, AdicionarTelefoneRequest request, CancellationToken ct)
        {
            var telefone = await _telefoneService.AdicionarAsync(pessoaId, request, ct);
            return CreatedAtAction(nameof(ObterPorPessoa), new { pessoaId }, telefone);
        }

        [HttpDelete("Excluir/{telefoneId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(Guid pessoaId, Guid telefoneId, CancellationToken ct)
        {
            await _telefoneService.RemoverAsync(pessoaId, telefoneId, ct);
            return NoContent();
        }
    }
}
