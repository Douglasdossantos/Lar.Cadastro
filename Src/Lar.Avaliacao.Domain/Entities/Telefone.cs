using Lar.Avaliacao.Domain.Enums;
using Lar.Avaliacao.Domain.Exceptions;

namespace Lar.Avaliacao.Domain.Entities
{
    public class Telefone
    {
        protected Telefone(){}
        public Guid Id { get; private set; }
        public Guid PessoaId { get; private set; }
        public TipoTelefone Tipo { get; private set; }
        public string Numero { get; private set; } = null!;

        public Telefone(Guid pessoaId, TipoTelefone tipo, string numero)
        {
            if (pessoaId == Guid.Empty)
                throw new DomainException("Telefone precisa informar o Id da Pessoa.");

            var numeroLimpo = new string(numero.Where(char.IsDigit).ToArray());

            if (numeroLimpo.Length is < 10 or > 11)
                throw new DomainException("Número de telefone deve ter entre 10 e 11 dígitos.");

            Id = Guid.NewGuid();
            PessoaId = pessoaId;
            Tipo = tipo;
            Numero = numeroLimpo;
        }
    }
}
