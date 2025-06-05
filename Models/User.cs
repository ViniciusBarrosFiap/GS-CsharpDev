using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_CSDev.Models
{
    //Classe que define o usuário
    public class User
    {
        public string Email { get; set; } = "";
        public string Senha { get; set; } = "";
        public string Role { get; set; } = ""; //Define o nivel de acesso do usuário
    }
}
