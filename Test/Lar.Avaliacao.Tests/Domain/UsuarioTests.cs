using FluentAssertions;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Exceptions;

namespace Lar.Avaliacao.Tests.Domain
{
    public class UsuarioTests
    {
        private const string HashValido = "100000.c2FsdA==.aGFzaA==";

        [Fact]
        public void Construtor_ComDadosValidos_DeveCriarUsuarioAtivo()
        {
            var usuario = new Usuario("Douglas Costa", "Douglas@Exemplo.com", HashValido);

            usuario.Id.Should().NotBeEmpty();
            usuario.Nome.Should().Be("Douglas Costa");
            usuario.Email.Should().Be("douglas@exemplo.com"); 
            usuario.SenhaHash.Should().Be(HashValido);
            usuario.Ativo.Should().BeTrue();
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Construtor_ComNomeInvalido_DeveLancarDomainException(string? nomeInvalido)
        {
            var act = () => new Usuario(nomeInvalido!, "douglas@exemplo.com", HashValido);

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Construtor_ComNomeMaiorQue250Caracteres_DeveLancarDomainException()
        {
            var act = () => new Usuario(new string('A', 251), "douglas@exemplo.com", HashValido);

            act.Should().Throw<DomainException>();
        }

        [Theory]
        [InlineData("")]
        [InlineData("sememail")]
        [InlineData("sem@arroba@duplicado.com")]
        [InlineData("@semusuario.com")]
        [InlineData("semdominio@")]
        public void Construtor_ComEmailInvalido_DeveLancarDomainException(string emailInvalido)
        {
            var act = () => new Usuario("Douglas", emailInvalido, HashValido);

            act.Should().Throw<DomainException>().WithMessage("*Email*");
        }

        [Fact]
        public void Construtor_ComSenhaHashVazia_DeveLancarDomainException()
        {
            var act = () => new Usuario("Douglas", "douglas@exemplo.com", "");

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void DefinirEmail_DeveNormalizarParaMinusculas()
        {
            var usuario = new Usuario("Douglas", "douglas@exemplo.com", HashValido);

            usuario.DefinirEmail("NOVO@EXEMPLO.COM");

            usuario.Email.Should().Be("novo@exemplo.com");
        }

        [Fact]
        public void Desativar_DeveMudarAtivoParaFalse()
        {
            var usuario = new Usuario("Douglas", "douglas@exemplo.com", HashValido);

            usuario.Desativar();

            usuario.Ativo.Should().BeFalse();
        }
    }
}
