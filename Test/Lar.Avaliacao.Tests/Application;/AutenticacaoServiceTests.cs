using FluentAssertions;
using Lar.Avaliacao.Application.Exceptions;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Lar.Avaliacao.Application.Services;
using Lar.Avaliacao.Domain.Entities;
using Lar.Avaliacao.Domain.Interfaces;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Lar.Avaliacao.Tests.Application_
{
    public class AutenticacaoServiceTests
    {
        private readonly Mock<IUsuarioRepository> _usuarioRepositoryMock = new();
        private readonly Mock<IPasswordHasher> _passwordHasherMock = new();
        private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly AutenticacaoService _service;
        public AutenticacaoServiceTests()
        {
            _service = new AutenticacaoService(
           _usuarioRepositoryMock.Object,
           _passwordHasherMock.Object,
           _jwtTokenGeneratorMock.Object,
           _unitOfWorkMock.Object,
           NullLogger<AutenticacaoService>.Instance);
        }


        [Fact]
        public async Task RegistrarAsync_ComEmailJaCadastrado_DeveLancarConflictException()
        {
            var request = new RegistrarUsuarioRequest { Nome = "Douglas", Email = "douglas@exemplo.com", Senha = "Senha123" };
            _usuarioRepositoryMock.Setup(r => r.ExisteComEmailAsync(request.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var act = async () => await _service.RegistrarAsync(request);

            await act.Should().ThrowAsync<ConflictException>();
            _usuarioRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task RegistrarAsync_ComDadosValidos_DeveGerarHashEPersistir()
        {
            var request = new RegistrarUsuarioRequest { Nome = "Douglas", Email = "douglas@exemplo.com", Senha = "Senha123" };
            _usuarioRepositoryMock.Setup(r => r.ExisteComEmailAsync(request.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
            _passwordHasherMock.Setup(h => h.GerarHash(request.Senha)).Returns("hash-gerado");

            var resultado = await _service.RegistrarAsync(request);

            resultado.Nome.Should().Be("Douglas");
            resultado.Email.Should().Be("douglas@exemplo.com");
            _passwordHasherMock.Verify(h => h.GerarHash(request.Senha), Times.Once);
            _usuarioRepositoryMock.Verify(r => r.AdicionarAsync(It.IsAny<Usuario>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SalvarAlteracoesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task LoginAsync_ComEmailNaoCadastrado_DeveLancarUnauthorizedException()
        {
            _usuarioRepositoryMock.Setup(r => r.ObterPorEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Usuario?)null);

            var act = async () => await _service.LoginAsync(new LoginRequest { Email = "x@x.com", Senha = "qualquer" });

            await act.Should().ThrowAsync<UnauthorizedException>();
        }

        [Fact]
        public async Task LoginAsync_ComSenhaIncorreta_DeveLancarUnauthorizedException()
        {
            var usuario = new Usuario("Douglas", "douglas@exemplo.com", "hash-armazenado");
            _usuarioRepositoryMock.Setup(r => r.ObterPorEmailAsync(usuario.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _passwordHasherMock.Setup(h => h.VerificarSenha("SenhaErrada", usuario.SenhaHash)).Returns(false);

            var act = async () => await _service.LoginAsync(new LoginRequest { Email = usuario.Email, Senha = "SenhaErrada" });

            await act.Should().ThrowAsync<UnauthorizedException>();
            _jwtTokenGeneratorMock.Verify(j => j.GerarToken(It.IsAny<Usuario>()), Times.Never);
        }

        [Fact]
        public async Task LoginAsync_ComUsuarioInativo_DeveLancarUnauthorizedExceptionMesmoComSenhaCorreta()
        {
            var usuario = new Usuario("Douglas", "douglas@exemplo.com", "hash-armazenado");
            usuario.Desativar();
            _usuarioRepositoryMock.Setup(r => r.ObterPorEmailAsync(usuario.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _passwordHasherMock.Setup(h => h.VerificarSenha("SenhaCorreta", usuario.SenhaHash)).Returns(true);

            var act = async () => await _service.LoginAsync(new LoginRequest { Email = usuario.Email, Senha = "SenhaCorreta" });

            await act.Should().ThrowAsync<UnauthorizedException>();
        }

        [Fact]
        public async Task LoginAsync_ComCredenciaisCorretas_DeveRetornarTokenEDadosDoUsuario()
        {
            var usuario = new Usuario("Douglas", "douglas@exemplo.com", "hash-armazenado");
            var expiraEm = DateTime.UtcNow.AddHours(1);
            _usuarioRepositoryMock.Setup(r => r.ObterPorEmailAsync(usuario.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);
            _passwordHasherMock.Setup(h => h.VerificarSenha("SenhaCorreta", usuario.SenhaHash)).Returns(true);
            _jwtTokenGeneratorMock.Setup(j => j.GerarToken(usuario)).Returns(("token-fake", expiraEm));

            var resultado = await _service.LoginAsync(new LoginRequest { Email = usuario.Email, Senha = "SenhaCorreta" });

            resultado.Token.Should().Be("token-fake");
            resultado.ExpiraEm.Should().Be(expiraEm);
            resultado.Usuario.Email.Should().Be(usuario.Email);
        }
    }
}
