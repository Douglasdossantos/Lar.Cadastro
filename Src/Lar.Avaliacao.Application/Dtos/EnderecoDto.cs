namespace Lar.Avaliacao.Application.Dtos
{
    public class EnderecoDto
    {
        public EnderecoDto(Guid id, string rua, string numero, string? complemento, string? referencia, string cidade, string estado)
        {
            Id = id;
            Rua = rua;
            Numero = numero;
            Complemento = complemento;
            Referencia = referencia;
            Cidade = cidade;
            Estado = estado;
        }

        public Guid Id { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string? Complemento { get; set; }
        public string? Referencia { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
    }
}
