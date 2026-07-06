using Lar.Avaliacao.Domain.Enums;

namespace Lar.Avaliacao.Application.Dtos
{
    public class TelefoneDto
    {
        public TelefoneDto(Guid id, TipoTelefone tipo, string numero)
        {
            Id = id;
            Tipo = tipo;
            Numero = numero;
        }

        public Guid Id { get; set; }
        public TipoTelefone Tipo { get; set; }
        public string Numero { get; set; }
    }
}
