using FluentAssertions;
using Lar.Avaliacao.Domain.Exceptions;
using Lar.Avaliacao.Domain.ValueObjects;

namespace Lar.Avaliacao.Tests.Domain
{
    public class CpfTests
    {
        [Theory]
        [InlineData("52998224725")]
        [InlineData("11144477735")]
        [InlineData("12345678909")]
        [InlineData("529.982.247-25")]
        public void Criar_ComCpfValido_DeveCriarInstancia(string cpfValido)
        {
            var cpf = Cpf.Criar(cpfValido);

            cpf.Should().NotBeNull();
            cpf.Numero.Should().HaveLength(11);
            cpf.Numero.Should().MatchRegex("^[0-9]+$");
        }

        [Fact]
        public void Criar_ComCpfVazio_DeveLancarDomainException()
        {
            var act = () => Cpf.Criar("");

            act.Should().Throw<DomainException>()
                .WithMessage("*vazio*");
        }

        [Fact]
        public void Criar_ComCpfNulo_DeveLancarDomainException()
        {
            var act = () => Cpf.Criar(null!);

            act.Should().Throw<DomainException>();
        }

        [Theory]
        [InlineData("123")]
        [InlineData("123456789012")]
        public void Criar_ComQuantidadeDigitosInvalida_DeveLancarDomainException(string cpfInvalido)
        {
            var act = () => Cpf.Criar(cpfInvalido);

            act.Should().Throw<DomainException>()
                .WithMessage("*11 dígitos*");
        }

        [Theory]
        [InlineData("00000000000")]
        [InlineData("11111111111")]
        [InlineData("99999999999")]
        public void Criar_ComDigitosRepetidos_DeveLancarDomainException(string cpfRepetido)
        {
            var act = () => Cpf.Criar(cpfRepetido);

            act.Should().Throw<DomainException>()
                .WithMessage("*inválido*");
        }

        [Fact]
        public void Criar_ComDigitoVerificadorInvalido_DeveLancarDomainException()
        {
            var act = () => Cpf.Criar("52998224700");

            act.Should().Throw<DomainException>()
                .WithMessage("*inválido*");
        }

        [Fact]
        public void Formatado_DeveRetornarComMascara()
        {
            var cpf = Cpf.Criar("52998224725");

            cpf.Formatado().Should().Be("529.982.247-25");
        }

        [Fact]
        public void Equals_ComMesmoNumero_DeveSerIgual()
        {
            var cpf1 = Cpf.Criar("52998224725");
            var cpf2 = Cpf.Criar("529.982.247-25");

            cpf1.Should().Be(cpf2);
            (cpf1 == cpf2).Should().BeFalse();
            cpf1.Equals(cpf2).Should().BeTrue();
            cpf1.GetHashCode().Should().Be(cpf2.GetHashCode());
        }

        [Fact]
        public void Equals_ComObjetoDeOutroTipo_DeveSerFalso()
        {
            var cpf = Cpf.Criar("52998224725");

            cpf.Equals("52998224725").Should().BeFalse();
            cpf.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void ToString_DeveRetornarApenasDigitos()
        {
            var cpf = Cpf.Criar("529.982.247-25");

            cpf.ToString().Should().Be("52998224725");
        }
    }
}
