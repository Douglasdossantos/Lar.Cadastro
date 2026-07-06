using Lar.Avaliacao.Domain.Exceptions;

namespace Lar.Avaliacao.Domain.Common
{
    public static class Guard
    {
        public static void ContraNuloOuVazio(string? valor, string nomeCampo)
        {
            if (string.IsNullOrWhiteSpace(valor))
                throw new DomainException($"{nomeCampo} é obrigatório.");
        }

        public static void ContraTamanhoForaDoIntervalo(string valor, int minimo, int maximo, string nomeCampo)
        {
            if (valor.Trim().Length < minimo || valor.Trim().Length > maximo)
            {
                var mensagem = minimo == maximo
                    ? $"{nomeCampo} deve ter exatamente {minimo} caracteres."
                    : $"{nomeCampo} deve ter entre {minimo} e {maximo} caracteres.";

                throw new DomainException(mensagem);
            }
        }

        public static void ContraMaiorQue(string? valor, int maximo, string nomeCampo)
        {
            if (valor is { Length: > 0 } && valor.Length > maximo)
                throw new DomainException($"{nomeCampo} deve ter no máximo {maximo} caracteres.");
        }

        public static void ContraGuidVazio(Guid valor, string nomeCampo)
        {
            if (valor == Guid.Empty)
                throw new DomainException($"{nomeCampo} é obrigatório.");
        }
    }
}
