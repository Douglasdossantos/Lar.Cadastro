using FluentAssertions;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Infra.Security;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;

namespace Lar.Avaliacao.Tests.Infra
{
    public class JwtTokenGeneratorTests
    {
        private static JwtTokenGenerator CriarGerador(int expiracaoMinutos = 60) =>
       new(Options.Create(new JwtSettings
       {
           Issuer = "CadastroApi.Testes",
           Audience = "CadastroApi.Testes.Clientes",
           SecretKey = "chave-secreta-de-teste-com-pelo-menos-32-caracteres!",
           ExpiracaoMinutos = expiracaoMinutos
       }));

        private static Usuario CriarUsuario() =>
            new("Douglas Costa", "douglas@exemplo.com", "100000.c2FsdA==.aGFzaA==");

        [Fact]
        public void GerarToken_DeveRetornarTokenNaoVazio()
        {
            var gerador = CriarGerador();
            var usuario = CriarUsuario();

            var (token, _) = gerador.GerarToken(usuario);

            token.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void GerarToken_DeveIncluirClaimsDoUsuario()
        {
            var gerador = CriarGerador();
            var usuario = CriarUsuario();

            var (token, _) = gerador.GerarToken(usuario);

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            jwt.Subject.Should().Be(usuario.Id.ToString());
            jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Email && c.Value == usuario.Email);
            jwt.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Name && c.Value == usuario.Nome);
        }

        [Fact]
        public void GerarToken_DeveConfigurarIssuerEAudienceCorretos()
        {
            var gerador = CriarGerador();
            var usuario = CriarUsuario();

            var (token, _) = gerador.GerarToken(usuario);

            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            jwt.Issuer.Should().Be("CadastroApi.Testes");
            jwt.Audiences.Should().Contain("CadastroApi.Testes.Clientes");
        }

        [Fact]
        public void GerarToken_ExpiraEmDeveSerCoerenteComConfiguracao()
        {
            var gerador = CriarGerador(expiracaoMinutos: 30);
            var usuario = CriarUsuario();

            var antes = DateTime.UtcNow;
            var (_, expiraEm) = gerador.GerarToken(usuario);
            var depois = DateTime.UtcNow;

            expiraEm.Should().BeOnOrAfter(antes.AddMinutes(30).AddSeconds(-5));
            expiraEm.Should().BeOnOrBefore(depois.AddMinutes(30).AddSeconds(5));
        }

        [Fact]
        public void GerarToken_ParaDoisUsuariosDiferentes_DeveGerarTokensDiferentes()
        {
            var gerador = CriarGerador();
            var usuario1 = CriarUsuario();
            var usuario2 = new Usuario("Maria", "maria@exemplo.com", "100000.c2FsdA==.aGFzaA==");

            var (token1, _) = gerador.GerarToken(usuario1);
            var (token2, _) = gerador.GerarToken(usuario2);

            token1.Should().NotBe(token2);
        }
    }
}
