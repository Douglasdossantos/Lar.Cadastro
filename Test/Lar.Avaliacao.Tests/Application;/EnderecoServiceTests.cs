using FluentAssertions;
using Lar.Avaliacao.Application.Exceptions;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Application.Services;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;


namespace Lar.Avaliacao.Tests.Application_
{
    public class EnderecoServiceTests
    {
        private const string CpfValido = "01234567890";
        private static readonly DateTime DataNascimentoValida = new(1995, 4, 10);

        private readonly Mock<IEnderecoRepository> _enderecoRepositoryMock = new();
        private readonly Mock<IPessoaRepository> _pessoaRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly EnderecoService _service;

        public EnderecoServiceTests()
        {
            _service = new EnderecoService(
                _enderecoRepositoryMock.Object,
                _pessoaRepositoryMock.Object,
                _unitOfWorkMock.Object,
                NullLogger<EnderecoService>.Instance
            );
        }

        [Fact]
        public async Task ObterPorPessoaIdAsync_QuandoPessoaNaoExiste_DeveLancarNotFoundException()
        {
            _pessoaRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Pessoa?)null);

            var act = async () => await _service.ObterPorPessoaIdAsync(Guid.NewGuid());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ObterPorPessoaIdAsync_QuandoPessoaExiste_DeveRetornarPessoaComEnderecos()
        {
            var pessoa = new Pessoa("Douglas", CpfValido, DataNascimentoValida);
            var enderecos = new List<Endereco>
        {
            new(pessoa.Id, "Avenida Brasil", "1234", "Apto 202", null, "Maringá", "PR"),
            new(pessoa.Id, "Rua das Flores", "156", null, "Perto da praça", "Maringá", "PR")
        };

            _pessoaRepositoryMock.Setup(r => r.ObterPorIdAsync(pessoa.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pessoa);
            _enderecoRepositoryMock.Setup(r => r.ObterPorPessoaIdAsync(pessoa.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(enderecos);

            var resultado = await _service.ObterPorPessoaIdAsync(pessoa.Id);

            resultado.Id.Should().Be(pessoa.Id);
            resultado.Endereco.Should().HaveCount(2);
            resultado.Endereco.Should().Contain(e => e.Rua == "Avenida Brasil" && e.Cidade == "Maringá");
        }

        [Fact]
        public async Task AdicionarAsync_QuandoPessoaNaoExiste_DeveLancarNotFoundExceptionSemPersistir()
        {
            _pessoaRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var request = new AdicionarEnderecoRequest
            {
                Rua = "Avenida Brasil",
                Numero = "1234",
                Cidade = "Maringá",
                Estado = "PR"
            };

            var act = async () => await _service.AdicionarAsync(Guid.NewGuid(), request);

            await act.Should().ThrowAsync<NotFoundException>();
            _enderecoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Endereco>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AdicionarAsync_QuandoPessoaExiste_DevePersistirERetornarDto()
        {
            var pessoaId = Guid.NewGuid();
            _pessoaRepositoryMock.Setup(r => r.ExisteAsync(pessoaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var request = new AdicionarEnderecoRequest
            {
                Rua = "Avenida Brasil",
                Numero = "1234",
                Complemento = "Apto 202",
                Referencia = "Próximo ao shopping",
                Cidade = "Maringá",
                Estado = "pr"
            };

            var resultado = await _service.AdicionarAsync(pessoaId, request);

            resultado.Rua.Should().Be("Avenida Brasil");
            resultado.Estado.Should().Be("PR");
            _enderecoRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Endereco>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
            _pessoaRepositoryMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RemoverAsync_QuandoEnderecoNaoExiste_DeveLancarNotFoundException()
        {
            _enderecoRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Endereco?)null);

            var act = async () => await _service.RemoverAsync(Guid.NewGuid(), Guid.NewGuid());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task RemoverAsync_QuandoEnderecoPertenceAOutraPessoa_DeveLancarNotFoundException()
        {
            var endereco = new Endereco(Guid.NewGuid(), "Rua A", "123", null, null, "Maringá", "PR");
            _enderecoRepositoryMock.Setup(r => r.ObterPorIdAsync(endereco.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(endereco);

            var act = async () => await _service.RemoverAsync(Guid.NewGuid(), endereco.Id);

            await act.Should().ThrowAsync<NotFoundException>();
            _enderecoRepositoryMock.Verify(r => r.Remover(It.IsAny<Endereco>()), Times.Never);
        }

        [Fact]
        public async Task RemoverAsync_QuandoEnderecoPertenceAPessoaCorreta_DeveRemoverESalvar()
        {
            var pessoaId = Guid.NewGuid();
            var endereco = new Endereco(pessoaId, "Rua A", "123", null, null, "Maringá", "PR");
            _enderecoRepositoryMock.Setup(r => r.ObterPorIdAsync(endereco.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(endereco);

            await _service.RemoverAsync(pessoaId, endereco.Id);

            _enderecoRepositoryMock.Verify(r => r.Remover(endereco), Times.Once);
            _unitOfWorkMock.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
