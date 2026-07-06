using FluentAssertions;
using Lar.Avaliacao.Application.Services;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Lar.Avaliacao.Tests.Application_
{
    public class PessoaServiceTests
    {
        private const string CpfValido = "52998224725";
        private static readonly DateTime DataNascimentoValida = new(1995, 4, 10);

        private readonly Mock<IPessoaRepository> _repositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly PessoaService _service;

        public PessoaServiceTests()
        {
            _service = new PessoaService(_repositoryMock.Object, _unitOfWorkMock.Object, NullLogger<PessoaService>.Instance);
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarTodasAsPessoasMapeadas()
        {
            var pessoas = new List<Pessoa>
        {
            new("Douglas", CpfValido, DataNascimentoValida),
            new("Maria", "11144477735", DataNascimentoValida)
        };
            _repositoryMock.Setup(r => r.ObterTodosAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(pessoas);

            var resultado = await _service.ObterTodosAsync();

            resultado.Should().HaveCount(2);
            resultado.Select(p => p.Nome).Should().Contain(new[] { "Douglas", "Maria" });
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoPessoaExiste_DeveRetornarDto()
        {
            var pessoa = new Pessoa("Douglas", CpfValido, DataNascimentoValida);
            _repositoryMock.Setup(r => r.ObterPorIdAsync(pessoa.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pessoa);

            var resultado = await _service.ObterPorIdAsync(pessoa.Id);

            resultado.Id.Should().Be(pessoa.Id);
            resultado.Nome.Should().Be("Douglas");
            resultado.Cpf.Should().Be(CpfValido);
        }

    }
}
