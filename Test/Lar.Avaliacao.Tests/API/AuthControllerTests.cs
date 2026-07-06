using FluentAssertions;
using Lar.Avaliacao.Api.Controllers;
using Lar.Avaliacao.Application.Dtos;
using Lar.Avaliacao.Application.Interfaces;
using Lar.Avaliacao.Application.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Lar.Avaliacao.Tests.API
{
    public class AuthControllerTests
    {
        private readonly Mock<IAutenticacaoService> _serviceMock = new();
        private readonly AuthController _controller;

        public AuthControllerTests()
        {
            _controller = new AuthController(_serviceMock.Object);
        }

        [Fact]
        public async Task Registrar_DeveRetornar201()
        {
            var request = new RegistrarUsuarioRequest { Nome = "Douglas", Email = "douglas@exemplo.com", Senha = "Senha123" };
            var usuarioDto = new UsuarioDto(Guid.NewGuid(), "Douglas", "douglas@exemplo.com");
            _serviceMock.Setup(s => s.RegistrarAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(usuarioDto);

            var resultado = await _controller.Registrar(request, CancellationToken.None);

            var createdResult = resultado.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
            createdResult.StatusCode.Should().Be(StatusCodes.Status201Created);
            createdResult.Value.Should().Be(usuarioDto);
        }

        [Fact]
        public async Task Login_DeveRetornar200ComToken()
        {
            var request = new LoginRequest { Email = "douglas@exemplo.com", Senha = "Senha123" };
            var usuarioDto = new UsuarioDto(Guid.NewGuid(), "Douglas", "douglas@exemplo.com");
            var loginResponse = new LoginResponseDto("token-fake", DateTime.UtcNow.AddHours(1), usuarioDto);
            _serviceMock.Setup(s => s.LoginAsync(request, It.IsAny<CancellationToken>())).ReturnsAsync(loginResponse);

            var resultado = await _controller.Login(request, CancellationToken.None);

            var okResult = resultado.Result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.Value.Should().Be(loginResponse);
        }
    }
}
