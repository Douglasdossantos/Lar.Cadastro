using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lar.Avaliacao.Application.Dtos
{
    public class LoginResponseDto
    {
        public LoginResponseDto(string token,  DateTime expiraEm, UsuarioDto usuario)
        {
            Token = token;
            ExpiraEm = expiraEm;
            Usuario = usuario;
        }

        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
        public UsuarioDto Usuario { get; set; }
    }
}
