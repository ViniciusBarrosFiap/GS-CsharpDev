using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//Classe responsável por registrar os logs internos da plataforma
namespace GS_CSDev.Models
{
    public class Eventos
    {
        public DateTime DataEvento { get; set; } //Data do log
        public string Acao { get; set; } //Ação realizada
        public string Descricao { get; set; } //Descrição da ação

    }
}
