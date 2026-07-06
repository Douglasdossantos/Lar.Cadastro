using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Application.Requests
{
    public record class AdicionarEnderecoRequest
    {
        public string Rua { get; init; } = string.Empty;
        public string Numero { get; init; } = string.Empty;
        public string? Complemento { get; init; }
        public string? Referencia { get; init; }
        public string Cidade { get; init; } = string.Empty;
        public string Estado { get; init; } = string.Empty;
    }
}
