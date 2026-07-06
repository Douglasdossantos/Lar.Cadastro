using Lar.Avaliacao.Domain.Exceptions;

namespace Lar.Avaliacao.Domain.Entities
{
    public class Endereco
    {
        public Endereco(
      Guid pessoaId,
      string rua,
      string numero,
      string? complemento,
      string? referencia,
      string cidade,
      string estado)
        {
            if (pessoaId == Guid.Empty)
                throw new DomainException("Endereço precisa informar o Id da Pessoa.");

            ValidarRua(rua);
            ValidarNumero(numero);
            ValidarComplemento(complemento);
            ValidarReferencia(referencia);
            ValidarCidade(cidade);
            ValidarEstado(estado);

            Id = Guid.NewGuid();
            PessoaId = pessoaId;
            Rua = rua.Trim();
            Numero = numero.Trim();
            Complemento = string.IsNullOrWhiteSpace(complemento) ? null : complemento.Trim();
            Referencia = string.IsNullOrWhiteSpace(referencia) ? null : referencia.Trim();
            Cidade = cidade.Trim();
            Estado = estado.Trim().ToUpperInvariant();
        }
        private Endereco()
        {
            
        }
        public Guid Id { get; private set; }
        public Guid PessoaId { get; private set; }
        public string Rua { get; private set; } = null!;
        public string Numero { get; private set; } = null!;
        public string? Complemento { get; private set; }
        public string? Referencia { get; private set; }
        public string Cidade { get; private set; } = null!;
        public string Estado { get; private set; } = null!;

        private static void ValidarRua(string rua)
        {
            if (string.IsNullOrWhiteSpace(rua) || rua.Trim().Length is < 3 or > 250)
                throw new DomainException("Rua deve ter entre 3 e 250 caracteres.");
        }

        private static void ValidarNumero(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero) || numero.Trim().Length is < 3 or > 10)
                throw new DomainException("Número deve ter entre 3 e 10 caracteres.");
        }

        private static void ValidarComplemento(string? complemento)
        {
            if (complemento is { Length: > 250 })
                throw new DomainException("Complemento deve ter no máximo 250 caracteres.");
        }

        private static void ValidarReferencia(string? referencia)
        {
            if (referencia is { Length: > 250 })
                throw new DomainException("Referência deve ter no máximo 250 caracteres.");
        }

        private static void ValidarCidade(string cidade)
        {
            if (string.IsNullOrWhiteSpace(cidade) || cidade.Trim().Length > 150)
                throw new DomainException("Cidade é obrigatória e deve ter no máximo 150 caracteres.");
        }

        private static void ValidarEstado(string estado)
        {
            if (string.IsNullOrWhiteSpace(estado) || estado.Trim().Length != 2)
                throw new DomainException("Estado deve ser a sigla com exatamente 2 caracteres (ex: PR, SP).");
        }
    }
}
