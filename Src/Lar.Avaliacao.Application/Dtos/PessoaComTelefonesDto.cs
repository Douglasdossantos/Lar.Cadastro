namespace Lar.Avaliacao.Application.Dtos
{
    public class PessoaComTelefonesDto
    {
        public PessoaComTelefonesDto(Guid id, string nome, bool ativo, IReadOnlyCollection<TelefoneDto> telefone)
        {
            Id = id;
            Nome = nome;           
            Ativo = ativo;
            Telefone = telefone;
        }

        public Guid Id { get; set; }
        public string Nome { get; set; }
        public bool Ativo { get; set; }
        public IReadOnlyCollection<TelefoneDto> Telefone { get; set; }
    }
}
