using FluentAssertions;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Exceptions;


namespace Lar.Avaliacao.Tests.Domain
{
    public class EnderecoTests
    {
        private static Guid PessoaId => Guid.NewGuid();

        [Fact]
        public void Construtor_ComDadosValidos_DeveCriarEndereco()
        {
            var pessoaId = PessoaId;

            var endereco = new Endereco(
                pessoaId, "Avenida Brasil", "1234", "Apto 202",
                "Próximo ao shopping", "Maringá", "pr");

            endereco.Id.Should().NotBeEmpty();
            endereco.PessoaId.Should().Be(pessoaId);
            endereco.Rua.Should().Be("Avenida Brasil");
            endereco.Numero.Should().Be("1234");
            endereco.Complemento.Should().Be("Apto 202");
            endereco.Referencia.Should().Be("Próximo ao shopping");
            endereco.Cidade.Should().Be("Maringá");
            endereco.Estado.Should().Be("PR"); 
        }

        [Fact]
        public void Construtor_ComComplementoEReferenciaNulos_DeveCriarComSucesso()
        {
            var endereco = new Endereco(PessoaId, "Rua das Flores", "123", null, null, "Maringá", "PR");

            endereco.Complemento.Should().BeNull();
            endereco.Referencia.Should().BeNull();
        }

        [Fact]
        public void Construtor_ComComplementoEReferenciaEmBranco_DeveNormalizarParaNulo()
        {
            var endereco = new Endereco(PessoaId, "Rua das Flores", "123", "   ", "", "Maringá", "PR");

            endereco.Complemento.Should().BeNull();
            endereco.Referencia.Should().BeNull();
        }

        [Fact]
        public void Construtor_ComPessoaIdVazio_DeveLancarDomainException()
        {
            var act = () => new Endereco(Guid.Empty, "Rua A", "123", null, null, "Maringá", "PR");

            act.Should().Throw<DomainException>()
                .WithMessage("*Id da Pessoa*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("AB")]
        public void Construtor_ComRuaInvalida_DeveLancarDomainException(string ruaInvalida)
        {
            var act = () => new Endereco(PessoaId, ruaInvalida, "123", null, null, "Maringá", "PR");

            act.Should().Throw<DomainException>()
                .WithMessage("*Rua*");
        }

        [Fact]
        public void Construtor_ComRuaMaiorQue250Caracteres_DeveLancarDomainException()
        {
            var ruaGrande = new string('A', 251);

            var act = () => new Endereco(PessoaId, ruaGrande, "123", null, null, "Maringá", "PR");

            act.Should().Throw<DomainException>()
                .WithMessage("*Rua*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("AB")]              
        [InlineData("12345678901")] 
        public void Construtor_ComNumeroInvalido_DeveLancarDomainException(string numeroInvalido)
        {
            var act = () => new Endereco(PessoaId, "Rua A", numeroInvalido, null, null, "Maringá", "PR");

            act.Should().Throw<DomainException>()
                .WithMessage("*Número*");
        }

        [Fact]
        public void Construtor_ComComplementoMaiorQue250Caracteres_DeveLancarDomainException()
        {
            var complementoGrande = new string('A', 251);

            var act = () => new Endereco(PessoaId, "Rua A", "123", complementoGrande, null, "Maringá", "PR");

            act.Should().Throw<DomainException>()
                .WithMessage("*Complemento*");
        }

        [Fact]
        public void Construtor_ComReferenciaMaiorQue250Caracteres_DeveLancarDomainException()
        {
            var referenciaGrande = new string('A', 251);

            var act = () => new Endereco(PessoaId, "Rua A", "123", null, referenciaGrande, "Maringá", "PR");

            act.Should().Throw<DomainException>()
                .WithMessage("*Referência*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        public void Construtor_ComCidadeInvalida_DeveLancarDomainException(string cidadeInvalida)
        {
            var act = () => new Endereco(PessoaId, "Rua A", "123", null, null, cidadeInvalida, "PR");

            act.Should().Throw<DomainException>()
                .WithMessage("*Cidade*");
        }

        [Fact]
        public void Construtor_ComCidadeMaiorQue150Caracteres_DeveLancarDomainException()
        {
            var cidadeGrande = new string('A', 151);

            var act = () => new Endereco(PessoaId, "Rua A", "123", null, null, cidadeGrande, "PR");

            act.Should().Throw<DomainException>()
                .WithMessage("*Cidade*");
        }

        [Theory]
        [InlineData("")]
        [InlineData("P")]
        [InlineData("PRR")]
        public void Construtor_ComEstadoInvalido_DeveLancarDomainException(string estadoInvalido)
        {
            var act = () => new Endereco(PessoaId, "Rua A", "123", null, null, "Maringá", estadoInvalido);

            act.Should().Throw<DomainException>()
                .WithMessage("*Estado*");
        }
    }
}
