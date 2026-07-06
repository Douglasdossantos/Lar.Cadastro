using FluentAssertions;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Enums;
using Lar.Avaliacao.Infra.Percistence.Repositories;

namespace Lar.Avaliacao.Tests.Infra
{
    public class TelefoneRepositoryTests : IClassFixture<SqliteInMemoryFixture>
    {
        private readonly SqliteInMemoryFixture _fixture;

        public TelefoneRepositoryTests(SqliteInMemoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AdicionarESalvar_DevePersistirTelefoneNoBanco()
        {
            await using var context = _fixture.CriarContexto();

            var pessoa = new Pessoa("Douglas", "52998224725", DateTime.Parse("2010-01-01"));

            context.Pessoas.Add(pessoa);
            await context.SaveChangesAsync();

            var repository = new TelefoneRepository(context);

            var telefone = new Telefone(
                pessoa.Id,
                TipoTelefone.Celular,
                "44999999999");

            await repository.AdicionarAsync(telefone);
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();

            var telefoneSalvo = await new TelefoneRepository(novoContexto)
                .ObterPorIdAsync(telefone.Id);

            telefoneSalvo.Should().NotBeNull();
            telefoneSalvo!.PessoaId.Should().Be(pessoa.Id);
            telefoneSalvo.Tipo.Should().Be(TipoTelefone.Celular);
            telefoneSalvo.Numero.Should().Be("44999999999");
        }

        [Fact]
        public async Task ObterPorPessoaIdAsync_DeveRetornarApenasTelefonesDaquelaPessoa()
        {
            await using var context = _fixture.CriarContexto();

            var pessoa = new Pessoa("Douglas", "18175460075", new DateTime(2001, 1, 1));
            var outraPessoa = new Pessoa("joazinho", "73582203077", new DateTime(2000,1,1));

            context.Pessoas.Add(pessoa);
            context.Pessoas.Add(outraPessoa);
            await context.SaveChangesAsync();

            var repository = new TelefoneRepository(context);

            await repository.AdicionarAsync(
                new Telefone(pessoa.Id, TipoTelefone.Celular, "44999999991"));

            await repository.AdicionarAsync(
                new Telefone(pessoa.Id, TipoTelefone.Residencial, "4433334444"));

            await repository.AdicionarAsync(
                new Telefone(outraPessoa.Id, TipoTelefone.Comercial, "4433335555"));

            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();

            var telefonesDaPessoa = await new TelefoneRepository(novoContexto)
                .ObterPorPessoaIdAsync(pessoa.Id);

            telefonesDaPessoa.Should().HaveCount(2);
            telefonesDaPessoa.Should().OnlyContain(t => t.PessoaId == pessoa.Id);
        }

        [Fact]
        public async Task Remover_DeveExcluirTelefoneDoBanco()
        {
            await using var context = _fixture.CriarContexto();

            var pessoa = new Pessoa(
                "Douglas",
                "44999144000",
                new DateTime(1990, 1, 1));

            context.Pessoas.Add(pessoa);
            await context.SaveChangesAsync();

            var repository = new TelefoneRepository(context);

            var telefone = new Telefone(
                pessoa.Id,
                TipoTelefone.Celular,
                "44999999999");

            await repository.AdicionarAsync(telefone);
            await context.SaveChangesAsync();

            repository.Remover(telefone);
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();

            var resultado = await new TelefoneRepository(novoContexto)
                .ObterPorIdAsync(telefone.Id);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new TelefoneRepository(context);

            var resultado = await repository.ObterPorIdAsync(Guid.NewGuid());

            resultado.Should().BeNull();
        }
    }
}
