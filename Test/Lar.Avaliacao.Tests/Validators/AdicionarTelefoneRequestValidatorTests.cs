using FluentValidation.TestHelper;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Application.Validators;
using Lar.Avaliacao.Domain.Enums;

namespace Lar.Avaliacao.Tests.Validators
{
    public class AdicionarTelefoneRequestValidatorTests
    {
        private readonly AdicionarTelefoneRequestValidator _validator = new();

        [Theory]
        [InlineData(TipoTelefone.Celular, "4499999999")]
        [InlineData(TipoTelefone.Residencial, "44999999999")]
        [InlineData(TipoTelefone.Comercial, "4433334444")]
        public void Validar_ComDadosValidos_NaoDeveTerErros(TipoTelefone tipo, string numero)
        {
            var resultado = _validator.TestValidate(new AdicionarTelefoneRequest { Tipo = tipo, Numero = numero });

            resultado.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validar_ComTipoInvalido_DeveTerErroEmTipo()
        {
            var request = new AdicionarTelefoneRequest { Tipo = (TipoTelefone)999, Numero = "44999999999" };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.Tipo);
        }

        [Theory]
        [InlineData("")]
        [InlineData("123456789")]      
        [InlineData("123456789012")]   
        [InlineData("4499999999A")]   
        public void Validar_ComNumeroInvalido_DeveTerErroEmNumero(string numeroInvalido)
        {
            var request = new AdicionarTelefoneRequest { Tipo = TipoTelefone.Celular, Numero = numeroInvalido };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.Numero);
        }
    }
}
