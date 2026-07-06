using Lar.Avaliacao.Domain.Common;
using Lar.Avaliacao.Domain.Exceptions;
using System.Text.RegularExpressions;

namespace Lar.Avaliacao.Domain.Entities
{
    public partial class Usuario
    {
        protected Usuario(){}
        public Guid Id { get; private set; }
        public string Nome { get; private set; } = null!;
        public string Email { get; private set; } = null!;
        public string SenhaHash { get; private set; } = null!;
        public bool Ativo { get; private set; }

        public Usuario(string nome, string email, string senhaHash)
        {
            Guard.ContraNuloOuVazio(nome, "Nome");
            Guard.ContraMaiorQue(nome, 250, "Nome");

            DefinirEmail(email);

            Guard.ContraNuloOuVazio(senhaHash, "Senha");

            Id = Guid.NewGuid();
            Nome = nome.Trim();
            SenhaHash = senhaHash;
            Ativo = true;
        }

        public void DefinirEmail(string email)
        {
            Guard.ContraNuloOuVazio(email, "Email");

            var emailNormalizado = email.Trim().ToLowerInvariant();

            if (!EmailRegex().IsMatch(emailNormalizado))
                throw new DomainException("Email possui formato inválido.");

            Email = emailNormalizado;
        }

        public void Desativar() => Ativo = false;

        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();
    }
}
