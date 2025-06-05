using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_CSDev.Models
{
    public class Ocorrencia
    {

        private static int contador = 1; //Contador para gerar Ids diferentes

        public int Id { get; set; } //Id da ocorrencia
        public string Local { get; set; } //Local da ocorrencia
        public DateTime Data { get; set; } //Data da ocorrencia
        public string Descricao { get; set; } 
        public string Status { get; set; } //Aberto, Resolvido, Finalizado

        public string EmailUsuario { get; set; } //Email do usuário que registrou a ocorrencia

        public Ocorrencia() //Construtor para atribuir um id
        {
            Id = contador++;
        }

        //Valida a entrada do usuário antes de registrar a ocorrencia
        public void ValidaEntradas()
        {
            if (string.IsNullOrWhiteSpace(Local)) // local não pode ser nulo
            {
                throw new ArgumentException("O campo 'Local' não pode ser vazio.");
            }
            if ( Data > DateTime.Now) //a data não pode ser no futuro
            {
                throw new Exception("A data não pode ser futura.");
            }
        }
    }
}
