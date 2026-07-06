using FluentAssertions;
using Lar.Avaliacao.Application.Exceptions;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Application.Services;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Enums;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Lar.Avaliacao.Tests.Application_
{
    public class TelefoneServiceTests
    {
        private const string CpfValido = "01234567890";
        private static readonly DateTime DataNascimentoValida = new(1995, 4, 10);

        private readonly Mock<ITelefoneRepository> _telefoneRepositoryMock = new();
        private readonly Mock<IPessoaRepository> _pessoaRepositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly TelefoneService _service;

        public TelefoneServiceTests()
        {
            _service = new TelefoneService(
                _telefoneRepositoryMock.Object,
                _pessoaRepositoryMock.Object,
                _unitOfWorkMock.Object,
                NullLogger<TelefoneService>.Instance
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
        public async Task ObterPorPessoaIdAsync_QuandoPessoaExiste_DeveRetornarPessoaComTelefones()
        {
            var pessoa = new Pessoa("Douglas", CpfValido, DataNascimentoValida);
            var telefones = new List<Telefone>
        {
            new(pessoa.Id, TipoTelefone.Celular, "44999999999"),
            new(pessoa.Id, TipoTelefone.Residencial, "4433334444")
        };

            _pessoaRepositoryMock.Setup(r => r.ObterPorIdAsync(pessoa.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(pessoa);
            _telefoneRepositoryMock.Setup(r => r.ObterPorPessoaIdAsync(pessoa.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(telefones);

            var resultado = await _service.ObterPorPessoaIdAsync(pessoa.Id);

            resultado.Id.Should().Be(pessoa.Id);
            resultado.Nome.Should().Be("Douglas");
            resultado.Telefone.Should().HaveCount(2);
            resultado.Telefone.Should().Contain(t => t.Tipo == TipoTelefone.Celular && t.Numero == "44999999999");
        }

        [Fact]
        public async Task AdicionarAsync_QuandoPessoaNaoExiste_DeveLancarNotFoundExceptionSemPersistir()
        {
            _pessoaRepositoryMock.Setup(r => r.ExisteAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            var request = new AdicionarTelefoneRequest { Tipo = TipoTelefone.Celular, Numero = "44999999999" };
            var act = async () => await _service.AdicionarAsync(Guid.NewGuid(), request);

            await act.Should().ThrowAsync<NotFoundException>();
            _telefoneRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Telefone>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task AdicionarAsync_QuandoPessoaExiste_DevePersistirERetornarDto()
        {
            var pessoaId = Guid.NewGuid();
            _pessoaRepositoryMock.Setup(r => r.ExisteAsync(pessoaId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var request = new AdicionarTelefoneRequest { Tipo = TipoTelefone.Comercial, Numero = "44999999999" };

            var resultado = await _service.AdicionarAsync(pessoaId, request);

            resultado.Tipo.Should().Be(TipoTelefone.Comercial);
            resultado.Numero.Should().Be("44999999999");
            _telefoneRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Telefone>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);

            _pessoaRepositoryMock.Verify(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RemoverAsync_QuandoTelefoneNaoExiste_DeveLancarNotFoundException()
        {
            _telefoneRepositoryMock.Setup(r => r.ObterPorIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Telefone?)null);

            var act = async () => await _service.RemoverAsync(Guid.NewGuid(), Guid.NewGuid());

            await act.Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task RemoverAsync_QuandoTelefonePertenceAOutraPessoa_DeveLancarNotFoundException()
        {
            var telefone = new Telefone(Guid.NewGuid(), TipoTelefone.Celular, "44999999999");
            _telefoneRepositoryMock.Setup(r => r.ObterPorIdAsync(telefone.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(telefone);

            var outraPessoaId = Guid.NewGuid();
            var act = async () => await _service.RemoverAsync(outraPessoaId, telefone.Id);

            await act.Should().ThrowAsync<NotFoundException>();
            _telefoneRepositoryMock.Verify(r => r.Remover(It.IsAny<Telefone>()), Times.Never);
        }

        [Fact]
        public async Task RemoverAsync_QuandoTelefonePertenceAPessoaCorreta_DeveRemoverESalvar()
        {
            var pessoaId = Guid.NewGuid();
            var telefone = new Telefone(pessoaId, TipoTelefone.Celular, "44999999999");
            _telefoneRepositoryMock.Setup(r => r.ObterPorIdAsync(telefone.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(telefone);

            await _service.RemoverAsync(pessoaId, telefone.Id);

            _telefoneRepositoryMock.Verify(r => r.Remover(telefone), Times.Once);
            _unitOfWorkMock.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
