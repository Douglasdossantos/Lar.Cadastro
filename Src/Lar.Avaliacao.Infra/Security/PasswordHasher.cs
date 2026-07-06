using Lar.Avaliacao.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Infra.Security
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int TamanhoSaltEmBytes = 16;
        private const int TamanhoHashEmBytes = 32;
        private const int Iteracoes = 100_000;
        private static readonly HashAlgorithmName Algoritmo = HashAlgorithmName.SHA256;

        public string GerarHash(string senha)
        {
            var salt = RandomNumberGenerator.GetBytes(TamanhoSaltEmBytes);
            var hash = Rfc2898DeriveBytes.Pbkdf2(senha, salt, Iteracoes, Algoritmo, TamanhoHashEmBytes);

            return $"{Iteracoes}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
        }

        public bool VerificarSenha(string senha, string hash)
        {
            var partes = hash.Split('.', 3);
            if (partes.Length != 3)
                return false;

            if (!int.TryParse(partes[0], out var iteracoes))
                return false;

            var salt = Convert.FromBase64String(partes[1]);
            var hashArmazenado = Convert.FromBase64String(partes[2]);

            var hashCalculado = Rfc2898DeriveBytes.Pbkdf2(senha, salt, iteracoes, Algoritmo, hashArmazenado.Length);

            return CryptographicOperations.FixedTimeEquals(hashCalculado, hashArmazenado);
        }
    }
}
