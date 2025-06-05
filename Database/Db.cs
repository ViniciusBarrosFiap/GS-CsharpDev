using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS_CSDev.Models;

//Classe simula bando de dados dos tickets a serem registrado
namespace GS_CSDev.Database
{
    public static class Db
    {
        public static List<Ocorrencia> Ocorrencias = new();
        public static List<Ticket> Tickets = new();
        public static List<Eventos> Eventos = new();
    }
}
