using FluentAssertions;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Enums;
using Lar.Avaliacao.Domain.Exceptions;

namespace Lar.Avaliacao.Tests.Domain
{
    public class TelefoneTests
    {
        [Theory]
        [InlineData("4499999999")]   
        [InlineData("44999999999")]  
        public void Construtor_ComDadosValidos_DeveCriarTelefone(string numero)
        {
            var pessoaId = Guid.NewGuid();

            var telefone = new Telefone(pessoaId, TipoTelefone.Celular, numero);

            telefone.Id.Should().NotBeEmpty();
            telefone.PessoaId.Should().Be(pessoaId);
            telefone.Tipo.Should().Be(TipoTelefone.Celular);
            telefone.Numero.Should().Be(numero);
        }

        [Fact]
        public void Construtor_DeveRemoverCaracteresNaoNumericosDoNumero()
        {
            var telefone = new Telefone(Guid.NewGuid(), TipoTelefone.Residencial, "(44) 99999-9999");

            telefone.Numero.Should().Be("44999999999");
        }

        [Fact]
        public void Construtor_ComPessoaIdVazio_DeveLancarDomainException()
        {
            var act = () => new Telefone(Guid.Empty, TipoTelefone.Comercial, "44999999999");

            act.Should().Throw<DomainException>()
                .WithMessage("*Id da Pessoa*");
        }

        [Theory]
        [InlineData("123456789")]     
        [InlineData("123456789012")] 
        [InlineData("")]
        public void Construtor_ComNumeroForaDoTamanhoPermitido_DeveLancarDomainException(string numeroInvalido)
        {
            var act = () => new Telefone(Guid.NewGuid(), TipoTelefone.Celular, numeroInvalido);

            act.Should().Throw<DomainException>()
                .WithMessage("*10 e 11 dígitos*");
        }

        [Theory]
        [InlineData(TipoTelefone.Celular)]
        [InlineData(TipoTelefone.Residencial)]
        [InlineData(TipoTelefone.Comercial)]
        public void Construtor_DeveAceitarTodosOsTiposDefinidos(TipoTelefone tipo)
        {
            var telefone = new Telefone(Guid.NewGuid(), tipo, "44999999999");

            telefone.Tipo.Should().Be(tipo);
        }
    }
}
