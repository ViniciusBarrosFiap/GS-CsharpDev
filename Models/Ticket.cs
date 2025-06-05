using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GS_CSDev.Models
{

//Classe ticket é como a concessionaria verá o ocorrido e herda atributos da classe ocorrencia
    public class Ticket : Ocorrencia
    {
        
        public int Id_ticket { get; set; }//Identificador
        public string Prioridade { get; set; } = ""; //Guarda a prioridade daquele ticket(baixo, média, alta)
        public DateTime DataTicket { get; set; } //Data que foi aberto o ticket


        //Método responsavel por simular uma IA usando regex e observando o nivel de urgencia de acordo com a descrição do ocorrido
        public void CalculaPrioridade()
        {
            try
            {
                string descricaoLower = Descricao.ToLower();
                var altaPrioridade = new Regex(@"\b(explodiu|morte|hospital|toxico|vida|apagão|sem luz)\b", RegexOptions.IgnoreCase); //Palavras de maior urgencia
                var mediaPrioridade = new Regex(@"\b(transformador|urgente|idoso|criança|cadeirante)\b", RegexOptions.IgnoreCase); //Palavras de média urgencia
                var baixaPrioridade = new Regex(@"\b(piscando|queda|10 minutos|dez minutos)\b"); //Palavras de baixa urgencia

                //mapeia a prioridade
                if (altaPrioridade.IsMatch(descricaoLower)){
                    Prioridade = "Alta";
                }
                else if (mediaPrioridade.IsMatch(descricaoLower)){
                    Prioridade = "Média";
                }
                else if (baixaPrioridade.IsMatch(descricaoLower)) {
                    Prioridade = "Baixa";
                }
                else
                {
                    Prioridade = "Indefinida";
                }
            }
            catch (Exception ex) {
                Prioridade = "Erro";
                Console.WriteLine(ex.Message);
            }

        }

        public void AlteraStatus(string novoStatus)
        {
            if (string.IsNullOrWhiteSpace(novoStatus))
            {
                throw new ArgumentException("O status não pode ser vazio.");
            }
            Status = novoStatus;
        }
    }


}
