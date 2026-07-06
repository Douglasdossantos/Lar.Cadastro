using FluentValidation.TestHelper;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Application.Validators;
using Xunit;


namespace Lar.Avaliacao.Tests.Validators
{
    public class AdicionarEnderecoRequestValidatorTests
    {
        private readonly AdicionarEnderecoRequestValidator _validator = new();

        private static AdicionarEnderecoRequest RequestValido() => new()
        {
            Rua = "Avenida Brasil",
            Numero = "1234",
            Complemento = "Apto 202",
            Referencia = "Próximo ao shopping",
            Cidade = "Maringá",
            Estado = "PR"
        };

        [Fact]
        public void Validar_ComDadosValidos_NaoDeveTerErros()
        {
            var resultado = _validator.TestValidate(RequestValido());

            resultado.ShouldNotHaveAnyValidationErrors();
        }

        //[Fact]
        //public void Validar_ComComplementoEReferenciaNulos_NaoDeveTerErros()
        //{
        //    var request = RequestValido();

        //    request.Complemento = null;
        //    // with { Complemento = null, Referencia = null };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldNotHaveAnyValidationErrors();
        //}

        //[Theory]
        //[InlineData("")]
        //[InlineData("AB")]
        //public void Validar_ComRuaInvalida_DeveTerErroEmRua(string ruaInvalida)
        //{
        //    var request = RequestValido() with { Rua = ruaInvalida };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldHaveValidationErrorFor(x => x.Rua);
        //}

        //[Fact]
        //public void Validar_ComRuaMaiorQue250Caracteres_DeveTerErroEmRua()
        //{
        //    var request = RequestValido() with { Rua = new string('A', 251) };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldHaveValidationErrorFor(x => x.Rua);
        //}

        //[Theory]
        //[InlineData("")]
        //[InlineData("AB")]
        //[InlineData("12345678901")]
        //public void Validar_ComNumeroInvalido_DeveTerErroEmNumero(string numeroInvalido)
        //{
        //    var request = RequestValido() with { Numero = numeroInvalido };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldHaveValidationErrorFor(x => x.Numero);
        //}

        //[Fact]
        //public void Validar_ComComplementoMaiorQue250Caracteres_DeveTerErroEmComplemento()
        //{
        //    var request = RequestValido() with { Complemento = new string('A', 251) };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldHaveValidationErrorFor(x => x.Complemento);
        //}

        //[Fact]
        //public void Validar_ComReferenciaMaiorQue250Caracteres_DeveTerErroEmReferencia()
        //{
        //    var request = RequestValido() with { Referencia = new string('A', 251) };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldHaveValidationErrorFor(x => x.Referencia);
        //}

        //[Theory]
        //[InlineData("")]
        //[InlineData("   ")]
        //public void Validar_ComCidadeInvalida_DeveTerErroEmCidade(string cidadeInvalida)
        //{
        //    var request = RequestValido() with { Cidade = cidadeInvalida };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldHaveValidationErrorFor(x => x.Cidade);
        //}

        //[Fact]
        //public void Validar_ComCidadeMaiorQue150Caracteres_DeveTerErroEmCidade()
        //{
        //    var request = RequestValido() with { Cidade = new string('A', 151) };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldHaveValidationErrorFor(x => x.Cidade);
        //}

        //[Theory]
        //[InlineData("")]
        //[InlineData("P")]
        //[InlineData("PRR")]
        //[InlineData("12")]
        //public void Validar_ComEstadoInvalido_DeveTerErroEmEstado(string estadoInvalido)
        //{
        //    var request = RequestValido() with { Estado = estadoInvalido };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldHaveValidationErrorFor(x => x.Estado);
        //}

        //[Fact]
        //public void Validar_ComEstadoMinusculo_NaoDeveTerErro()
        //{
        //    var request = RequestValido() with { Estado = "pr" };

        //    var resultado = _validator.TestValidate(request);

        //    resultado.ShouldNotHaveValidationErrorFor(x => x.Estado);
        //}
    }
}
