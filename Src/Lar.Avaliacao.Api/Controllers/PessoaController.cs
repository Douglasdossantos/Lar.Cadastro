using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lar.Avaliacao.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    //[AllowAnonymous]
    [Authorize]
    public class PessoaController : ControllerBase
    {
        private readonly IPessoaService _pessoaService;

        public PessoaController(IPessoaService pessoaService)
        {
            _pessoaService = pessoaService;
        }

        [HttpGet("ObterTodos")]
        [ProducesResponseType(typeof(IEnumerable<PessoaDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<PessoaDto>>> ObterTodos(CancellationToken ct)
        {
            var pessoas = await _pessoaService.ObterTodosAsync(ct);
            return Ok(pessoas);
        }

        [HttpGet("Pesquisar/{id:guid}")]
        [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PessoaDto>> ObterPorId(Guid id, CancellationToken ct)
        {
            var pessoa = await _pessoaService.ObterPorIdAsync(id, ct);
            return Ok(pessoa);
        }

        [HttpPost("Criar")]
        [ProducesResponseType(typeof(PessoaDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<PessoaDto>> Criar(CriarPessoaRequest request, CancellationToken ct)
        {
            var pessoa = await _pessoaService.CriarAsync(request, ct);
            return CreatedAtAction(nameof(ObterPorId), new { id = pessoa.Id }, pessoa);
        }

        [HttpPut("Atualizar/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Atualizar(Guid id, AtualizarPessoaRequest request, CancellationToken ct)
        {
            await _pessoaService.AtualizarAsync(id, request, ct);
            return NoContent();
        }

        [HttpDelete("Excluir/{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Remover(Guid id, CancellationToken ct)
        {
            await _pessoaService.RemoverAsync(id, ct);
            return NoContent();
        }
    }
}
