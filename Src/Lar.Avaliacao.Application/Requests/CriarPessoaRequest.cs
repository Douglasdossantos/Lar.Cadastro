using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Application.Requests
{
    public class CriarPessoaRequest
    {
        public string Nome { get; init; } = string.Empty;
        public string Cpf { get; init; } = string.Empty;
        public DateTime DataNascimento { get; init; }
    }
}
