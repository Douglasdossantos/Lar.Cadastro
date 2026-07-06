using FluentValidation.TestHelper;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Application.Validators;

namespace Lar.Avaliacao.Tests.Validators
{
    public class LoginRequestValidatorTests
    {
        private readonly LoginRequestValidator _validator = new();

        [Fact]
        public void Validar_ComDadosValidos_NaoDeveTerErros()
        {
            var resultado = _validator.TestValidate(new LoginRequest { Email = "douglas@exemplo.com", Senha = "qualquer" });

            resultado.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validar_ComEmailInvalido_DeveTerErroEmEmail()
        {
            var resultado = _validator.TestValidate(new LoginRequest { Email = "invalido", Senha = "qualquer" });

            resultado.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Validar_ComSenhaVazia_DeveTerErroEmSenha()
        {
            var resultado = _validator.TestValidate(new LoginRequest { Email = "douglas@exemplo.com", Senha = "" });

            resultado.ShouldHaveValidationErrorFor(x => x.Senha);
        }
    }
}
