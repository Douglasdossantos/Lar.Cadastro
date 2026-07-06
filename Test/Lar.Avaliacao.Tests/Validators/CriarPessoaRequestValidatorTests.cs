using FluentValidation.TestHelper;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Application.Validators;
using Lar.Avaliacao.Tests.TestHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Tests.Validators
{
    public class CriarPessoaRequestValidatorTests
    {
    
        private readonly CriarPessoaRequestValidator _validator = new(FakeClock.Em(2026, 7, 3));

        [Fact]
        public void Validar_ComDadosValidos_NaoDeveTerErros()
        {
            var request = new CriarPessoaRequest
            {
                Nome = "Douglas Costa",
                Cpf = "52998224725",
                DataNascimento = new DateTime(1995, 4, 10)
            };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validar_ComNomeVazio_DeveTerErroEmNome()
        {
            var request = new CriarPessoaRequest { Nome = "", Cpf = "52998224725", DataNascimento = new DateTime(1995, 4, 10) };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.Nome);
        }

        [Fact]
        public void Validar_ComNomeMaiorQue250Caracteres_DeveTerErroEmNome()
        {
            var request = new CriarPessoaRequest
            {
                Nome = new string('A', 251),
                Cpf = "52998224725",
                DataNascimento = new DateTime(1995, 4, 10)
            };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.Nome);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123")]
        [InlineData("123456789012")]
        [InlineData("5299822472A")]
        public void Validar_ComCpfInvalido_DeveTerErroEmCpf(string cpfInvalido)
        {
            var request = new CriarPessoaRequest { Nome = "Douglas", Cpf = cpfInvalido, DataNascimento = new DateTime(1995, 4, 10) };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.Cpf);
        }

        [Fact]
        public void Validar_ComDataNascimentoNaoInformada_DeveTerErro()
        {
            var request = new CriarPessoaRequest { Nome = "Douglas", Cpf = "52998224725", DataNascimento = default };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.DataNascimento);
        }

        [Fact]
        public void Validar_ComDataNascimentoFutura_DeveTerErro()
        {
            var request = new CriarPessoaRequest
            {
                Nome = "Douglas",
                Cpf = "52998224725",
                DataNascimento = new DateTime(2026, 7, 4) 
            };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.DataNascimento);
        }

        [Fact]
        public void Validar_ComDataNascimentoIgualAHoje_DeveTerErro()
        {
            var request = new CriarPessoaRequest
            {
                Nome = "Douglas",
                Cpf = "52998224725",
                DataNascimento = new DateTime(2026, 7, 3) 
            };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.DataNascimento);
        }
    }
}
