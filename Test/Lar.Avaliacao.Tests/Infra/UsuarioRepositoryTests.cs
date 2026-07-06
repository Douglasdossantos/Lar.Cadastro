using FluentAssertions;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Infra.Percistence.Repositories;

namespace Lar.Avaliacao.Tests.Infra
{
    public class UsuarioRepositoryTests : IClassFixture<SqliteInMemoryFixture>
    {
        private readonly SqliteInMemoryFixture _fixture;

        public UsuarioRepositoryTests(SqliteInMemoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AdicionarESalvar_DevePersistirUsuarioNoBanco()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new UsuarioRepository(context);
            var usuario = new Usuario("Douglas", "douglas.repo@exemplo.com", "100000.c2FsdA==.aGFzaA==");

            await repository.AdicionarAsync(usuario);
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();
            var usuarioSalvo = await new UsuarioRepository(novoContexto).ObterPorEmailAsync("douglas.repo@exemplo.com");

            usuarioSalvo.Should().NotBeNull();
            usuarioSalvo!.Nome.Should().Be("Douglas");
        }

        [Fact]
        public async Task ObterPorEmailAsync_DeveSerCaseInsensitive()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new UsuarioRepository(context);
            await repository.AdicionarAsync(new Usuario("Maria", "maria.repo@exemplo.com", "100000.c2FsdA==.aGFzaA=="));
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();
            var usuario = await new UsuarioRepository(novoContexto).ObterPorEmailAsync("MARIA.REPO@EXEMPLO.COM");

            usuario.Should().NotBeNull();
        }

        [Fact]
        public async Task ObterPorEmailAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new UsuarioRepository(context);

            var usuario = await repository.ObterPorEmailAsync("naoexiste@exemplo.com");

            usuario.Should().BeNull();
        }

        [Fact]
        public async Task ExisteComEmailAsync_QuandoExiste_DeveRetornarTrue()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new UsuarioRepository(context);
            await repository.AdicionarAsync(new Usuario("Ana", "ana.repo@exemplo.com", "100000.c2FsdA==.aGFzaA=="));
            await context.SaveChangesAsync();

            var existe = await repository.ExisteComEmailAsync("ana.repo@exemplo.com");

            existe.Should().BeTrue();
        }

        [Fact]
        public async Task ExisteComEmailAsync_QuandoNaoExiste_DeveRetornarFalse()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new UsuarioRepository(context);

            var existe = await repository.ExisteComEmailAsync("inexistente@exemplo.com");

            existe.Should().BeFalse();
        }
    }
}
