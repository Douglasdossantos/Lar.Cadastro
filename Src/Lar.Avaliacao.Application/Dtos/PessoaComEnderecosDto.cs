using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Application.Dtos
{
    public class PessoaComEnderecosDto
    {
        public PessoaComEnderecosDto(Guid id, string nome, bool ativo, IReadOnlyCollection<EnderecoDto> endereco)
        {
            Id = id;
            Nome = nome;
            Ativo = ativo;
            Endereco = endereco;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }

        public IReadOnlyCollection<EnderecoDto> Endereco { get; set; }
    }
}
