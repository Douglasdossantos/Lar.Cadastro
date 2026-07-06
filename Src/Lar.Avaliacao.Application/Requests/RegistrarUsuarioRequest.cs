
namespace Lar.Avaliacao.Application.Requests
{
    public class RegistrarUsuarioRequest
    {
        public string Nome { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Senha { get; init; } = string.Empty;
    }
}
