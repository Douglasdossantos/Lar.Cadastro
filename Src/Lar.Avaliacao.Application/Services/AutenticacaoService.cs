using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Exceptions;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Lar.Avaliacao.Application.Services
{
    public class AutenticacaoService : IAutenticacaoService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<AutenticacaoService> _logger;

        public AutenticacaoService(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IUnitOfWork unitOfWork, ILogger<AutenticacaoService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _jwtTokenGenerator = jwtTokenGenerator;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginRequest request, CancellationToken ct = default)
        {
            var usuario = await _usuarioRepository.ObterPorEmailAsync(request.Email, ct);

            if (usuario is null || !usuario.Ativo || !_passwordHasher.VerificarSenha(request.Senha, usuario.SenhaHash))
            {
                _logger.LogWarning("Tentativa de login inválida para o email: {Email}", request.Email);
                throw new UnauthorizedException("Email ou senha inválidos.");
            }

            var (token, expiraEm) = _jwtTokenGenerator.GerarToken(usuario);

            _logger.LogInformation("Login realizado com sucesso: {UsuarioId} ({Email})", usuario.Id, usuario.Email);

            return new LoginResponseDto(token, expiraEm, new UsuarioDto(usuario.Id, usuario.Nome, usuario.Email));
        }

        public async Task<UsuarioDto> RegistrarAsync(RegistrarUsuarioRequest request, CancellationToken ct = default)
        {
            if (await _usuarioRepository.ExisteComEmailAsync(request.Email, ct))
            {
                _logger.LogWarning("Tentativa de registro com email já cadastrado: {Email}", request.Email);
                throw new ConflictException($"Já existe um usuário cadastrado com o email '{request.Email}'.");
            }

            var hash = _passwordHasher.GerarHash(request.Senha);
            var usuario = new Usuario(request.Nome, request.Email, hash);

            await _usuarioRepository.AdicionarAsync(usuario, ct);
            await _unitOfWork.SalvarAlteracoesAsync(ct);

            _logger.LogInformation("Novo usuário registrado: {UsuarioId} ({Email})", usuario.Id, usuario.Email);

            return new UsuarioDto(usuario.Id, usuario.Nome, usuario.Email);
        }
    }
}
