using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Exceptions;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.Extensions.Logging;


namespace Lar.Avaliacao.Application.Services
{
    public class EnderecoService : IEnderecoService
    {
        private readonly IEnderecoRepository _enderecoRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<EnderecoService> _logger;

        public EnderecoService(
            IEnderecoRepository enderecoRepository,
            IPessoaRepository pessoaRepository,
            IUnitOfWork unitOfWork,
            ILogger<EnderecoService> logger)
        {
            _enderecoRepository = enderecoRepository;
            _pessoaRepository = pessoaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<EnderecoDto> AdicionarAsync(
            Guid pessoaId, AdicionarEnderecoRequest request, CancellationToken ct = default)
        {
            if (!await _pessoaRepository.ExisteAsync(pessoaId, ct))
            {
                _logger.LogWarning("Tentativa de adicionar endereço para Pessoa inexistente: {PessoaId}", pessoaId);
                throw new NotFoundException($"Pessoa com id '{pessoaId}' não foi encontrada.");
            }

            var endereco = new Endereco(
                pessoaId, request.Rua, request.Numero, request.Complemento,
                request.Referencia, request.Cidade, request.Estado);

            await _enderecoRepository.AdicionarAsync(endereco, ct);
            await _unitOfWork.SalvarAlteracoesAsync(ct);

            _logger.LogInformation("Endereço {EnderecoId} adicionado para Pessoa {PessoaId}", endereco.Id, pessoaId);

            return new EnderecoDto(
                endereco.Id, endereco.Rua, endereco.Numero,
                endereco.Complemento, endereco.Referencia, endereco.Cidade, endereco.Estado);
        }

        public async Task<PessoaComEnderecosDto> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken ct = default)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(pessoaId, ct)
                ?? throw new NotFoundException($"Pessoa com id '{pessoaId}' não foi encontrada.");

            var enderecos = await _enderecoRepository.ObterPorPessoaIdAsync(pessoaId, ct);

            var enderecosDto = enderecos
                .Select(e => new EnderecoDto(e.Id, e.Rua, e.Numero, e.Complemento, e.Referencia, e.Cidade, e.Estado))
                .ToList();

            return new PessoaComEnderecosDto(
                pessoa.Id, pessoa.Nome,pessoa.Ativo, enderecosDto);
        }

        public async Task RemoverAsync(Guid pessoaId, Guid enderecoId, CancellationToken ct = default)
        {
            var endereco = await _enderecoRepository.ObterPorIdAsync(enderecoId, ct)
                ?? throw new NotFoundException($"Endereço com id '{enderecoId}' não foi encontrado.");

            if (endereco.PessoaId != pessoaId)
                throw new NotFoundException($"Endereço '{enderecoId}' não pertence à pessoa '{pessoaId}'.");

            _enderecoRepository.Remover(endereco);
            await _unitOfWork.SalvarAlteracoesAsync(ct);

            _logger.LogInformation("Endereço {EnderecoId} removido da Pessoa {PessoaId}", enderecoId, pessoaId);
        }
    }
}
