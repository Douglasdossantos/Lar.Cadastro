using FluentAssertions;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Exceptions;

namespace Lar.Avaliacao.Tests.Domain
{
    public class PessoaTests
    {
        private const string CpfValido = "52998224725";
        private static readonly DateTime DataNascimentoValida = new(1995, 4, 10);

        [Fact]
        public void Construtor_ComDadosValidos_DeveCriarPessoaAtiva()
        {
            var pessoa = new Pessoa("Douglas Costa", CpfValido, DataNascimentoValida);

            pessoa.Id.Should().NotBeEmpty();
            pessoa.Nome.Should().Be("Douglas Costa");
            pessoa.Cpf.Numero.Should().Be(CpfValido);
            pessoa.DataNascimento.Should().Be(DataNascimentoValida.Date);
            pessoa.Ativo.Should().BeTrue();
        }

        [Fact]
        public void Construtor_DeveGerarIdsDiferentesParaCadaInstancia()
        {
            var pessoa1 = new Pessoa("Nome 1", CpfValido, DataNascimentoValida);
            var pessoa2 = new Pessoa("Nome 2", "11144477735", DataNascimentoValida);

            pessoa1.Id.Should().NotBe(pessoa2.Id);
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData(null)]
        public void Construtor_ComNomeInvalido_DeveLancarDomainException(string? nomeInvalido)
        {
            var act = () => new Pessoa(nomeInvalido!, CpfValido, DataNascimentoValida);

            act.Should().Throw<DomainException>()
                .WithMessage("*Nome*obrigatório*");
        }

        [Fact]
        public void Construtor_ComNomeMaiorQue250Caracteres_DeveLancarDomainException()
        {
            var nomeGrande = new string('A', 251);

            var act = () => new Pessoa(nomeGrande, CpfValido, DataNascimentoValida);

            act.Should().Throw<DomainException>()
                .WithMessage("*250*");
        }

        [Fact]
        public void Construtor_ComNomeDeExatamente250Caracteres_DeveCriarComSucesso()
        {
            var nomeLimite = new string('A', 250);

            var pessoa = new Pessoa(nomeLimite, CpfValido, DataNascimentoValida);

            pessoa.Nome.Should().HaveLength(250);
        }

        [Fact]
        public void Construtor_ComCpfInvalido_DeveLancarDomainException()
        {
            var act = () => new Pessoa("Douglas", "123", DataNascimentoValida);

            act.Should().Throw<DomainException>();
        }

        [Fact]
        public void Construtor_ComDataNascimentoHoje_DeveLancarDomainException()
        {
            var act = () => new Pessoa("Douglas", CpfValido, DateTime.Today);

            act.Should().Throw<DomainException>()
                .WithMessage("*passada*");
        }

        [Fact]
        public void Construtor_ComDataNascimentoFutura_DeveLancarDomainException()
        {
            var act = () => new Pessoa("Douglas", CpfValido, DateTime.Today.AddDays(1));

            act.Should().Throw<DomainException>()
                .WithMessage("*passada*");
        }

        [Fact]
        public void DefinirNome_ComNomeValido_DeveAtualizarNome()
        {
            var pessoa = new Pessoa("Nome Antigo", CpfValido, DataNascimentoValida);

            pessoa.DefinirNome("Nome Novo");

            pessoa.Nome.Should().Be("Nome Novo");
        }

        [Fact]
        public void DefinirNome_DeveRemoverEspacosEmBrancoDasExtremidades()
        {
            var pessoa = new Pessoa("Nome Antigo", CpfValido, DataNascimentoValida);

            pessoa.DefinirNome("  Nome Com Espaco  ");

            pessoa.Nome.Should().Be("Nome Com Espaco");
        }

        [Fact]
        public void DefinirNome_ComNomeInvalido_DeveLancarDomainExceptionEManterNomeAntigo()
        {
            var pessoa = new Pessoa("Nome Antigo", CpfValido, DataNascimentoValida);

            var act = () => pessoa.DefinirNome("");

            act.Should().Throw<DomainException>();
            pessoa.Nome.Should().Be("Nome Antigo");
        }

        [Fact]
        public void Desativar_DevoMudarAtivoParaFalse()
        {
            var pessoa = new Pessoa("Douglas", CpfValido, DataNascimentoValida);

            pessoa.Desativar();

            pessoa.Ativo.Should().BeFalse();
        }

        [Fact]
        public void Ativar_ApósDesativar_DeveVoltarParaTrue()
        {
            var pessoa = new Pessoa("Douglas", CpfValido, DataNascimentoValida);
            pessoa.Desativar();

            pessoa.Ativar();

            pessoa.Ativo.Should().BeTrue();
        }
    }
}
