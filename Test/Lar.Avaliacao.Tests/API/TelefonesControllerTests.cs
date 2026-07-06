using FluentAssertions;
using Lar.Avaliacao.Api.Controllers;
using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lar.Avaliacao.Tests.API
{
    public class TelefonesControllerTests
    {
        private readonly Mock<ITelefoneService> _serviceMock = new();
        private readonly TelefoneController _controller;
        public TelefonesControllerTests()
        {
            _controller = new TelefoneController(_serviceMock.Object);
        }

        [Fact]
        public async Task ObterPorPessoa_DeveRetornar200ComPessoaETelefones()
        {
            var pessoaId = Guid.NewGuid();
            var dto = new PessoaComTelefonesDto(
                pessoaId, "Douglas", true,
                new List<TelefoneDto> { new(Guid.NewGuid(), TipoTelefone.Celular, "44999999999") });

            _serviceMock.Setup(s => s.ObterPorPessoaIdAsync(pessoaId, It.IsAny<CancellationToken>())).ReturnsAsync(dto);

            var resultado = await _controller.ObterPorPessoa(pessoaId, CancellationToken.None);

            var okResult = resultado.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(dto);
        }

        [Fact]
        public async Task Adicionar_DeveRetornar201()
        {
            var pessoaId = Guid.NewGuid();
            var request = new AdicionarTelefoneRequest { Tipo = TipoTelefone.Celular, Numero = "44999999999" };
            var telefoneDto = new TelefoneDto(Guid.NewGuid(), TipoTelefone.Celular, "44999999999");

            _serviceMock.Setup(s => s.AdicionarAsync(pessoaId, request, It.IsAny<CancellationToken>())).ReturnsAsync(telefoneDto);

            var resultado = await _controller.Adicionar(pessoaId, request, CancellationToken.None);

            var createdResult = resultado.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            createdResult.Value.Should().Be(telefoneDto);
        }

        [Fact]
        public async Task Remover_DeveRetornar204()
        {
            var pessoaId = Guid.NewGuid();
            var telefoneId = Guid.NewGuid();

            var resultado = await _controller.Remover(pessoaId, telefoneId, CancellationToken.None);

            resultado.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.RemoverAsync(pessoaId, telefoneId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
