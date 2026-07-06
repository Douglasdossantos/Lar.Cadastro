using FluentAssertions;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Infra.Percistence.Repositories;

namespace Lar.Avaliacao.Tests.Infra
{
    public class PessoaRepositoryTests : IClassFixture<SqliteInMemoryFixture>
    {
        private readonly SqliteInMemoryFixture _fixture;
        private static readonly DateTime DataNascimentoValida = new(1995, 4, 10);

        public PessoaRepositoryTests(SqliteInMemoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AdicionarESalvar_DevePersistirPessoaNoBanco()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new PessoaRepository(context);
            var pessoa = new Pessoa("Douglas", "52998224725", DataNascimentoValida);

            await repository.AdicionarAsync(pessoa);
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();
            var pessoaSalva = await new PessoaRepository(novoContexto).ObterPorIdAsync(pessoa.Id);

            pessoaSalva.Should().NotBeNull();
            pessoaSalva!.Nome.Should().Be("Douglas");
            pessoaSalva.Cpf.Numero.Should().Be("52998224725");
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new PessoaRepository(context);

            var resultado = await repository.ObterPorIdAsync(Guid.NewGuid());

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ExisteAsync_QuandoPessoaExiste_DeveRetornarTrue()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new PessoaRepository(context);
            var pessoa = new Pessoa("Maria", "11144477735", DataNascimentoValida);
            await repository.AdicionarAsync(pessoa);
            await context.SaveChangesAsync();

            var existe = await repository.ExisteAsync(pessoa.Id);

            existe.Should().BeTrue();
        }

        [Fact]
        public async Task ExisteAsync_QuandoPessoaNaoExiste_DeveRetornarFalse()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new PessoaRepository(context);

            var existe = await repository.ExisteAsync(Guid.NewGuid());

            existe.Should().BeFalse();
        }

        [Fact]
        public async Task ExisteComCpfAsync_ComCpfJaCadastrado_DeveRetornarTrueMesmoComMascara()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new PessoaRepository(context);
            await repository.AdicionarAsync(new Pessoa("Ana", "98765432100", DataNascimentoValida));
            await context.SaveChangesAsync();

            var existe = await repository.ExisteComCpfAsync("987.654.321-00");

            existe.Should().BeTrue();
        }

        [Fact]
        public async Task ExisteComCpfAsync_ComCpfNaoCadastrado_DeveRetornarFalse()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new PessoaRepository(context);

            var existe = await repository.ExisteComCpfAsync("12345678909");

            existe.Should().BeFalse();
        }

        [Fact]
        public async Task Atualizar_ComEntidadeCarregadaNoMesmoContexto_DevePersistirAlteracao()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new PessoaRepository(context);
            var pessoa = new Pessoa("Nome Original", "52998224806", DataNascimentoValida);
            await repository.AdicionarAsync(pessoa);
            await context.SaveChangesAsync();

            pessoa.DefinirNome("Nome Atualizado");
            repository.Atualizar(pessoa);
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();
            var pessoaAtualizada = await new PessoaRepository(novoContexto).ObterPorIdAsync(pessoa.Id);
            pessoaAtualizada!.Nome.Should().Be("Nome Atualizado");
        }

        [Fact]
        public async Task Remover_DeveExcluirPessoaDoBanco()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new PessoaRepository(context);
            var pessoa = new Pessoa("Para Remover", "12345678909", DataNascimentoValida);
            await repository.AdicionarAsync(pessoa);
            await context.SaveChangesAsync();

            repository.Remover(pessoa);
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();
            var resultado = await new PessoaRepository(novoContexto).ObterPorIdAsync(pessoa.Id);
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarTodasAsPessoasCadastradas()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new PessoaRepository(context);
            await repository.AdicionarAsync(new Pessoa("Pessoa A", "18889910038", DataNascimentoValida));
            await repository.AdicionarAsync(new Pessoa("Pessoa B", "71403444005", DataNascimentoValida));
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();
            var todas = await new PessoaRepository(novoContexto).ObterTodosAsync();

            todas.Select(p => p.Nome).Should().Contain(new[] { "Pessoa A", "Pessoa B" });
        }
    }
}
