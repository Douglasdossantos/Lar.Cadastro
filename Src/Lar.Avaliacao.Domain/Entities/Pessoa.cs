using Lar.Avaliacao.Domain.Exceptions;
using Lar.Avaliacao.Domain.ValueObjects;

namespace Lar.Avaliacao.Domain.Entities
{
    public class Pessoa
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; } = null!;
        public Cpf Cpf { get; private set; } = null!;
        public DateTime DataNascimento { get; private set; }
        public bool Ativo { get; private set; }
        private Pessoa()
        {
            
        }
        public Pessoa(string nome, string cpf, DateTime dataNascimento)
        {
            DefinirNome(nome);
            Cpf = Cpf.Criar(cpf);
            DefinirDataNascimento(dataNascimento);

            Id = Guid.NewGuid();
            Ativo = true;
        }

        public void DefinirNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new DomainException("Nome é obrigatório.");

            if (nome.Trim().Length > 250)
                throw new DomainException("Nome deve ter no máximo 250 caracteres.");

            Nome = nome.Trim();
        }

        private void DefinirDataNascimento(DateTime dataNascimento)
        {
            if (dataNascimento.Date >= DateTime.Today)
                throw new DomainException("Data de nascimento deve ser uma data passada.");

            DataNascimento = dataNascimento.Date;
        }

        public void Ativar() => Ativo = true;

        public void Desativar() => Ativo = false;
    }
}
