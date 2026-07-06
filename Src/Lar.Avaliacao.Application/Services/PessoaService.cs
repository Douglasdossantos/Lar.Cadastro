using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Exceptions;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PessoaService> _logger;

        public PessoaService(IPessoaRepository repository, IUnitOfWork unitOfWork, ILogger<PessoaService> logger)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task AtualizarAsync(Guid id, AtualizarPessoaRequest request, CancellationToken ct = default)
        {
            var pessoa = await ObterPessoaOuFalharAsync(id, ct);

            pessoa.DefinirNome(request.Nome);

            _repository.Atualizar(pessoa);
            await _unitOfWork.SalvarAlteracoesAsync(ct);

            _logger.LogInformation("Pessoa atualizada: {PessoaId}", pessoa.Id);
        }

        public async Task<PessoaDto> CriarAsync(CriarPessoaRequest request, CancellationToken ct = default)
        {
            if (await _repository.ExisteComCpfAsync(request.Cpf, ct))
            {
                _logger.LogWarning("Tentativa de criar Pessoa com CPF já cadastrado: {Cpf}", request.Cpf);
                throw new ConflictException($"Já existe uma pessoa cadastrada com o CPF {request.Cpf}.");
            }

            var pessoa = new Pessoa(request.Nome, request.Cpf, request.DataNascimento);

            await _repository.AdicionarAsync(pessoa, ct);
            await _unitOfWork.SalvarAlteracoesAsync(ct);

            _logger.LogInformation("Pessoa criada: {PessoaId}", pessoa.Id);

            return MapearParaDto(pessoa);
        }

        public async Task<PessoaDto> ObterPorIdAsync(Guid id, CancellationToken ct = default)
        {
            var pessoa = await ObterPessoaOuFalharAsync(id, ct);
            return MapearParaDto(pessoa);
        }

        public async Task<IEnumerable<PessoaDto>> ObterTodosAsync(CancellationToken ct = default)
        {
            var pessoas = await _repository.ObterTodosAsync(ct);
            return pessoas.Select(MapearParaDto);
        }

        public async Task RemoverAsync(Guid id, CancellationToken ct = default)
        {
            var pessoa = await ObterPessoaOuFalharAsync(id, ct);

            _repository.Remover(pessoa);
            await _unitOfWork.SalvarAlteracoesAsync(ct);

            _logger.LogInformation("Pessoa removida: {PessoaId}", pessoa.Id);
        }
        private async Task<Pessoa> ObterPessoaOuFalharAsync(Guid id, CancellationToken ct)
        {
            return await _repository.ObterPorIdAsync(id, ct)
                ?? throw new NotFoundException($"Pessoa com id '{id}' não foi encontrada.");
        }

        private static PessoaDto MapearParaDto(Pessoa pessoa) =>
            new(pessoa.Id, pessoa.Nome, pessoa.Cpf.Numero, pessoa.DataNascimento, pessoa.Ativo);
    }
}
