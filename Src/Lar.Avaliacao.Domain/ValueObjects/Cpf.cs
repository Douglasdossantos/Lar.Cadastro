using Lar.Avaliacao.Domain.Exceptions;

namespace Lar.Avaliacao.Domain.ValueObjects
{
    public sealed class Cpf : IEquatable<Cpf>
    {
        public string Numero { get; }

        private Cpf(string numero)
        {
            Numero = numero;
        }

        public static Cpf Criar(string numero)
        {
            if (string.IsNullOrWhiteSpace(numero))
                throw new DomainException("CPF não pode ser vazio.");

            var apenasDigitos = new string(numero.Where(char.IsDigit).ToArray());

            if (apenasDigitos.Length != 11)
                throw new DomainException("CPF deve conter exatamente 11 dígitos.");

            if (!ValidarDigitosVerificadores(apenasDigitos))
                throw new DomainException("CPF inválido.");

            return new Cpf(apenasDigitos);
        }

        private static bool ValidarDigitosVerificadores(string cpf)
        {
            if (cpf.Distinct().Count() == 1)
                return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            var tempCpf = cpf[..9];
            var soma = tempCpf.Select((c, i) => (c - '0') * multiplicador1[i]).Sum();
            var resto = soma % 11;
            var digito1 = resto < 2 ? 0 : 11 - resto;

            tempCpf += digito1;
            soma = tempCpf.Select((c, i) => (c - '0') * multiplicador2[i]).Sum();
            resto = soma % 11;
            var digito2 = resto < 2 ? 0 : 11 - resto;

            return cpf.EndsWith($"{digito1}{digito2}");
        }

        public string Formatado() =>
            Convert.ToUInt64(Numero).ToString(@"000\.000\.000\-00");

        public override string ToString() => Numero;

        public bool Equals(Cpf? other) => other is not null && Numero == other.Numero;

        public override bool Equals(object? obj) => Equals(obj as Cpf);

        public override int GetHashCode() => Numero.GetHashCode();
    }
}
