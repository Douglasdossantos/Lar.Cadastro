using FluentAssertions;
using Lar.Avaliacao.Api.Controllers;
using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lar.Avaliacao.Tests.API
{
    public class PessoasControllerTests
    {
        private readonly Mock<IPessoaService> _serviceMock = new();
        private readonly PessoaController _controller;
        public PessoasControllerTests()
        {
            _controller = new PessoaController(_serviceMock.Object);
        }

        [Fact]
        public async Task ObterTodos_DeveRetornar200ComListaDePessoas()
        {
            var pessoas = new List<PessoaDto>
        {
            new(Guid.NewGuid(), "Douglas", "52998224725", new DateTime(1995, 4, 10), true)
        };
            _serviceMock.Setup(s => s.ObterTodosAsync(It.IsAny<CancellationToken>())).ReturnsAsync(pessoas);

            var resultado = await _controller.ObterTodos(CancellationToken.None);

            var okResult = resultado.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);
            okResult.Value.Should().BeEquivalentTo(pessoas);
        }

        [Fact]
        public async Task ObterPorId_DeveRetornar200ComAPessoa()
        {
            var pessoaDto = new PessoaDto(Guid.NewGuid(), "Douglas", "52998224725", new DateTime(1995, 4, 10), true);
            _serviceMock.Setup(s => s.ObterPorIdAsync(pessoaDto.Id, It.IsAny<CancellationToken>())).ReturnsAsync(pessoaDto);

            var resultado = await _controller.ObterPorId(pessoaDto.Id, CancellationToken.None);

            var okResult = resultado.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(pessoaDto);
        }

        [Fact]
        public async Task Criar_DeveRetornar201ComLocationHeader()
        {
            var request = new CriarPessoaRequest { Nome = "Douglas", Cpf = "52998224725", DataNascimento = new DateTime(1995, 4, 10) };
            var pessoaDto = new PessoaDto(Guid.NewGuid(), "Douglas", "52998224725", new DateTime(1995, 4, 10), true);
            _serviceMock.Setup(s => s.CriarAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(pessoaDto);

            var resultado = await _controller.Criar(request, CancellationToken.None);

            var createdResult = resultado.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            createdResult.ActionName.Should().Be(nameof(PessoaController.ObterPorId));
            createdResult.Value.Should().Be(pessoaDto);
        }

        [Fact]
        public async Task Atualizar_DeveRetornar204()
        {
            var id = Guid.NewGuid();
            var request = new AtualizarPessoaRequest { Nome = "Novo Nome" };

            var resultado = await _controller.Atualizar(id, request, CancellationToken.None);

            resultado.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.AtualizarAsync(id, request, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Remover_DeveRetornar204()
        {
            var id = Guid.NewGuid();

            var resultado = await _controller.Remover(id, CancellationToken.None);

            resultado.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.RemoverAsync(id, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
