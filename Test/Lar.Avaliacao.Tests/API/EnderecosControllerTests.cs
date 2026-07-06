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
    public class EnderecosControllerTests
    {
        private readonly Mock<IEnderecoService> _serviceMock = new();
        private readonly EnderecoController _controller;

        public EnderecosControllerTests()
        {
            _controller = new EnderecoController(_serviceMock.Object);
        }

        [Fact]
        public async Task ObterPorPessoa_DeveRetornar200ComPessoaEEnderecos()
        {
            var pessoaId = Guid.NewGuid();
            var dto = new PessoaComEnderecosDto(
                pessoaId, "Douglas", true,
                new List<EnderecoDto> { new(Guid.NewGuid(), "Avenida Brasil", "1234", null, null, "Maringá", "PR") });

            _serviceMock.Setup(s => s.ObterPorPessoaIdAsync(pessoaId, It.IsAny<CancellationToken>())).ReturnsAsync(dto);

            var resultado = await _controller.ObterPorPessoa(pessoaId, CancellationToken.None);

            var okResult = resultado.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(dto);
        }

        [Fact]
        public async Task Adicionar_DeveRetornar201()
        {
            var pessoaId = Guid.NewGuid();
            var request = new AdicionarEnderecoRequest
            {
                Rua = "Avenida Brasil",
                Numero = "1234",
                Cidade = "Maringá",
                Estado = "PR"
            };
            var enderecoDto = new EnderecoDto(Guid.NewGuid(), "Avenida Brasil", "1234", null, null, "Maringá", "PR");

            _serviceMock.Setup(s => s.AdicionarAsync(pessoaId, request, It.IsAny<CancellationToken>())).ReturnsAsync(enderecoDto);

            var resultado = await _controller.Adicionar(pessoaId, request, CancellationToken.None);

            var createdResult = resultado.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            createdResult.Value.Should().Be(enderecoDto);
        }

        [Fact]
        public async Task Remover_DeveRetornar204()
        {
            var pessoaId = Guid.NewGuid();
            var enderecoId = Guid.NewGuid();

            var resultado = await _controller.Remover(pessoaId, enderecoId, CancellationToken.None);

            resultado.Should().BeOfType<NoContentResult>();
            _serviceMock.Verify(s => s.RemoverAsync(pessoaId, enderecoId, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
