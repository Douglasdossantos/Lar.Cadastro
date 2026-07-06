using Lar.Avaliacao.Domain.Enums;

namespace Lar.Avaliacao.Application.Requests
{
    public class AdicionarTelefoneRequest
    {
        public TipoTelefone Tipo { get; init; }
        public string Numero { get; init; } = string.Empty;
    }
}
