using FluentAssertions;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Infra.Percistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Tests.Infra
{
    public class EnderecoRepositoryTests  : IClassFixture<SqliteInMemoryFixture>
    {
        private readonly SqliteInMemoryFixture _fixture;

        public EnderecoRepositoryTests(SqliteInMemoryFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task AdicionarESalvar_DevePersistirEnderecoNoBanco()
        {
            await using var context = _fixture.CriarContexto();

            var cpf = "90555173054";

            var pessoa = new Pessoa(
                "Douglas",
                cpf,
                new DateTime(1990, 1, 1));

            context.Pessoas.Add(pessoa);
            await context.SaveChangesAsync();

            var repository = new EnderecoRepository(context);

            var endereco = new Endereco(
                pessoa.Id,
                "Avenida Brasil",
                "1234",
                "Apto 202",
                null,
                "Maringá",
                "PR");

            await repository.AdicionarAsync(endereco);
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();

            var repositoryConsulta = new EnderecoRepository(novoContexto);

            var enderecoSalvo = await repositoryConsulta.ObterPorIdAsync(endereco.Id);

            enderecoSalvo.Should().NotBeNull();
            enderecoSalvo!.PessoaId.Should().Be(pessoa.Id);
            enderecoSalvo.Rua.Should().Be("Avenida Brasil");
            enderecoSalvo.Numero.Should().Be("1234");
            enderecoSalvo.Cidade.Should().Be("Maringá");
            enderecoSalvo.Estado.Should().Be("PR");
        }

        [Fact]
        public async Task ObterPorPessoaIdAsync_DeveRetornarApenasEnderecosDaquelaPessoa()
        {
            await using var context = _fixture.CriarContexto();

            var pessoa = new Pessoa(
                "Douglas",
                "12345678909",
                new DateTime(1990, 1, 1));

            var outraPessoa = new Pessoa(
                "Maria",
                "98765432100",
                new DateTime(1992, 5, 10));

            context.Pessoas.Add(pessoa);
            context.Pessoas.Add(outraPessoa);
            await context.SaveChangesAsync();

            var repository = new EnderecoRepository(context);

            await repository.AdicionarAsync(
                new Endereco(pessoa.Id, "Rua A", "111", null, null, "Maringá", "PR"));

            await repository.AdicionarAsync(
                new Endereco(pessoa.Id, "Rua B", "222", null, null, "Maringá", "PR"));

            await repository.AdicionarAsync(
                new Endereco(outraPessoa.Id, "Rua C", "333", null, null, "Sarandi", "PR"));

            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();

            var enderecosDaPessoa = await new EnderecoRepository(novoContexto)
                .ObterPorPessoaIdAsync(pessoa.Id);

            enderecosDaPessoa.Should().HaveCount(2);
            enderecosDaPessoa.Should().OnlyContain(e => e.PessoaId == pessoa.Id);
        }

        [Fact]
        public async Task Remover_DeveExcluirEnderecoDoBanco()
        {
            var cpf ="48039314020";
           await using var context = _fixture.CriarContexto();

            var pessoa = new Pessoa(
                "Douglas",
                cpf,
                new DateTime(1990, 1, 1));

            context.Pessoas.Add(pessoa);
            await context.SaveChangesAsync();

            var repository = new EnderecoRepository(context);

            var endereco = new Endereco(
                pessoa.Id,
                "Rua A",
                "123",
                null,
                null,
                "Maringá",
                "PR");

            await repository.AdicionarAsync(endereco);
            await context.SaveChangesAsync();

            repository.Remover(endereco);
            await context.SaveChangesAsync();

            await using var novoContexto = _fixture.CriarContexto();

            var resultado = await new EnderecoRepository(novoContexto)
                .ObterPorIdAsync(endereco.Id);

            resultado.Should().BeNull();
        }

        [Fact]
        public async Task ObterPorIdAsync_QuandoNaoExiste_DeveRetornarNull()
        {
            await using var context = _fixture.CriarContexto();
            var repository = new EnderecoRepository(context);

            var resultado = await repository.ObterPorIdAsync(Guid.NewGuid());

            resultado.Should().BeNull();
        }
    }
}
