using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Exceptions;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Lar.Avaliacao.Application.Services
{
    public class TelefoneService : ITelefoneService
    {
        private readonly ITelefoneRepository _telefoneRepository;
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TelefoneService> _logger;

        public TelefoneService(ITelefoneRepository telefoneRepository, IPessoaRepository pessoaRepository, IUnitOfWork unitOfWork, ILogger<TelefoneService> logger)
        {
            _telefoneRepository = telefoneRepository;
            _pessoaRepository = pessoaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<TelefoneDto> AdicionarAsync(Guid pessoaId, AdicionarTelefoneRequest request, CancellationToken ct = default)
        {
            if (!await _pessoaRepository.ExisteAsync(pessoaId, ct))
            {
                _logger.LogWarning("Tentativa de adicionar telefone para Pessoa inexistente: {PessoaId}", pessoaId);
                throw new NotFoundException($"Pessoa com id '{pessoaId}' não foi encontrada.");
            }

            var telefone = new Telefone(pessoaId, request.Tipo, request.Numero);

            await _telefoneRepository.AdicionarAsync(telefone, ct);
            await _unitOfWork.SalvarAlteracoesAsync(ct);

            _logger.LogInformation("Telefone {TelefoneId} adicionado para Pessoa {PessoaId}", telefone.Id, pessoaId);

            return new TelefoneDto(telefone.Id, telefone.Tipo, telefone.Numero);
        }

        public async Task<PessoaComTelefonesDto> ObterPorPessoaIdAsync(Guid pessoaId, CancellationToken ct = default)
        {
            var pessoa = await _pessoaRepository.ObterPorIdAsync(pessoaId, ct)
                 ?? throw new NotFoundException($"Pessoa com id '{pessoaId}' não foi encontrada.");

            var telefones = await _telefoneRepository.ObterPorPessoaIdAsync(pessoaId, ct);

            var telefonesDto = telefones
                .Select(t => new TelefoneDto(t.Id, t.Tipo, t.Numero))
                .ToList();

            return new PessoaComTelefonesDto(
                pessoa.Id, pessoa.Nome, pessoa.Ativo, telefonesDto);
        }

        public async Task RemoverAsync(Guid pessoaId, Guid telefoneId, CancellationToken ct = default)
        {
            var telefone = await _telefoneRepository.ObterPorIdAsync(telefoneId, ct)
            ?? throw new NotFoundException($"Telefone com id '{telefoneId}' não foi encontrado.");

            if (telefone.PessoaId != pessoaId)
                throw new NotFoundException($"Telefone '{telefoneId}' não pertence à pessoa '{pessoaId}'.");

            _telefoneRepository.Remover(telefone);
            await _unitOfWork.SalvarAlteracoesAsync(ct);

            _logger.LogInformation("Telefone {TelefoneId} removido da Pessoa {PessoaId}", telefoneId, pessoaId);
        }
    }
}
