using FluentValidation.TestHelper;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Application.Validators;

namespace Lar.Avaliacao.Tests.Validators
{
    public class RegistrarUsuarioRequestValidatorTests
    {
        private readonly RegistrarUsuarioRequestValidator _validator = new();

        [Fact]
        public void Validar_ComDadosValidos_NaoDeveTerErros()
        {
            var request = new RegistrarUsuarioRequest { Nome = "Douglas", Email = "douglas@exemplo.com", Senha = "Senha123" };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldNotHaveAnyValidationErrors();
        }

        [Fact]
        public void Validar_ComNomeVazio_DeveTerErroEmNome()
        {
            var request = new RegistrarUsuarioRequest { Nome = "", Email = "douglas@exemplo.com", Senha = "Senha123" };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.Nome);
        }

        [Theory]
        [InlineData("")]
        [InlineData("sememail")]
        [InlineData("sem-arroba.com")]
        public void Validar_ComEmailInvalido_DeveTerErroEmEmail(string emailInvalido)
        {
            var request = new RegistrarUsuarioRequest { Nome = "Douglas", Email = emailInvalido, Senha = "Senha123" };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Theory]
        [InlineData("")]
        [InlineData("curta1")]        
        [InlineData("somenteletras")]
        [InlineData("12345678")]
        public void Validar_ComSenhaInvalida_DeveTerErroEmSenha(string senhaInvalida)
        {
            var request = new RegistrarUsuarioRequest { Nome = "Douglas", Email = "douglas@exemplo.com", Senha = senhaInvalida };

            var resultado = _validator.TestValidate(request);

            resultado.ShouldHaveValidationErrorFor(x => x.Senha);
        }
    }
}
