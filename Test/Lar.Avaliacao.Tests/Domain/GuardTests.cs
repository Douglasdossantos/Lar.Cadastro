using FluentAssertions;
using Lar.Avaliacao.Domain.Common;
using Lar.Avaliacao.Domain.Exceptions;

namespace Lar.Avaliacao.Tests.Domain
{
    public class GuardTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void ContraNuloOuVazio_ComValorInvalido_DeveLancarDomainException(string? valorInvalido)
        {
            var act = () => Guard.ContraNuloOuVazio(valorInvalido, "Campo");

            act.Should().Throw<DomainException>().WithMessage("Campo é obrigatório.");
        }

        [Fact]
        public void ContraNuloOuVazio_ComValorValido_NaoDeveLancar()
        {
            var act = () => Guard.ContraNuloOuVazio("valor", "Campo");

            act.Should().NotThrow();
        }

        [Fact]
        public void ContraTamanhoForaDoIntervalo_ComValorMenorQueMinimo_DeveLancar()
        {
            var act = () => Guard.ContraTamanhoForaDoIntervalo("ab", 3, 10, "Campo");

            act.Should().Throw<DomainException>().WithMessage("*entre 3 e 10*");
        }

        [Fact]
        public void ContraTamanhoForaDoIntervalo_ComValorMaiorQueMaximo_DeveLancar()
        {
            var act = () => Guard.ContraTamanhoForaDoIntervalo(new string('a', 11), 3, 10, "Campo");

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void ContraTamanhoForaDoIntervalo_ComMinimoIgualAoMaximo_DeveUsarMensagemDeTamanhoExato()
        {
            var act = () => Guard.ContraTamanhoForaDoIntervalo("abc", 2, 2, "Estado");

            act.Should().Throw<DomainException>().WithMessage("*exatamente 2*");
        }

        [Fact]
        public void ContraTamanhoForaDoIntervalo_ComValorDentroDoIntervalo_NaoDeveLancar()
        {
            var act = () => Guard.ContraTamanhoForaDoIntervalo("abcde", 3, 10, "Campo");

            act.Should().NotThrow();
        }

        [Fact]
        public void ContraMaiorQue_ComValorMaiorQueMaximo_DeveLancar()
        {
            var act = () => Guard.ContraMaiorQue(new string('a', 11), 10, "Campo");

            act.Should().Throw<DomainException>().WithMessage("*máximo 10*");
        }

        [Fact]
        public void ContraMaiorQue_ComValorNulo_NaoDeveLancar()
        {
            var act = () => Guard.ContraMaiorQue(null, 10, "Campo");

            act.Should().NotThrow();
        }

        [Fact]
        public void ContraMaiorQue_ComValorDentroDoLimite_NaoDeveLancar()
        {
            var act = () => Guard.ContraMaiorQue("abc", 10, "Campo");

            act.Should().NotThrow();
        }

        [Fact]
        public void ContraGuidVazio_ComGuidVazio_DeveLancar()
        {
            var act = () => Guard.ContraGuidVazio(Guid.Empty, "PessoaId");

            act.Should().Throw<DomainException>().WithMessage("PessoaId é obrigatório.");
        }

        [Fact]
        public void ContraGuidVazio_ComGuidValido_NaoDeveLancar()
        {
            var act = () => Guard.ContraGuidVazio(Guid.NewGuid(), "PessoaId");

            act.Should().NotThrow();
        }
    }
}
