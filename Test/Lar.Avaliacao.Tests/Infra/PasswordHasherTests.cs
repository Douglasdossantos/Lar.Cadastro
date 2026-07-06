using FluentAssertions;
using Lar.Avaliacao.Infra.Security;

namespace Lar.Avaliacao.Tests.Infra
{
    public class PasswordHasherTests
    {
        private readonly PasswordHasher _hasher = new();

        [Fact]
        public void GerarHash_DeveRetornarStringComTresPartesSeparadasPorPonto()
        {
            var hash = _hasher.GerarHash("MinhaSenha123");

            hash.Split('.').Should().HaveCount(3);
        }

        [Fact]
        public void GerarHash_ChamadoDuasVezesComMesmaSenha_DeveGerarHashesDiferentes()
        {
            var hash1 = _hasher.GerarHash("MinhaSenha123");
            var hash2 = _hasher.GerarHash("MinhaSenha123");

            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void VerificarSenha_ComSenhaCorreta_DeveRetornarTrue()
        {
            var hash = _hasher.GerarHash("MinhaSenha123");

            _hasher.VerificarSenha("MinhaSenha123", hash).Should().BeTrue();
        }

        [Fact]
        public void VerificarSenha_ComSenhaIncorreta_DeveRetornarFalse()
        {
            var hash = _hasher.GerarHash("MinhaSenha123");

            _hasher.VerificarSenha("SenhaErrada", hash).Should().BeFalse();
        }

        [Theory]
        [InlineData("formato-invalido")]
        [InlineData("apenas.duaspartes")]
        [InlineData("abc.def.ghi.jkl")]
        public void VerificarSenha_ComHashEmFormatoInvalido_DeveRetornarFalseSemLancarExcecao(string hashInvalido)
        {
            var act = () => _hasher.VerificarSenha("qualquer", hashInvalido);

            act.Should().NotThrow();
            _hasher.VerificarSenha("qualquer", hashInvalido).Should().BeFalse();
        }
    }
}
