using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GS_CSDev.Database;
using GS_CSDev.Models;
using OfficeOpenXml;
using System.IO;
namespace GS_CSDev.Services
{

    //Classe principal por lidar com ações dos consumidores e Administradores
    public class GerenciaOcorrencia
    {
        private readonly User usuarioAutenticado; //armazena o usuário autenticado

        public GerenciaOcorrencia(User user) //Inicia a váriavel com o usuário autenticado(construtor)
        {
            usuarioAutenticado = user;
        }

        //Método que já registra uma ocorrencia e abre o ticket para a concessionária
        public void RegistraOcorrenciaTicket()
        {
            AdicionarOcorrencia();
            GerarTicket();
        }

        //Método para criar e salvar uma nova ocorrência
        public void AdicionarOcorrencia()
        {
            try
            {
                Console.WriteLine("Digite o local da ocorrência:");
                string local = Console.ReadLine();

                Console.WriteLine("Informe a data:");
                DateTime data = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("Descreva a ocorrência:");
                string descricao = Console.ReadLine();

                //Cria um objeto ocorrencia com as informações e associa ela ao usuário
                Ocorrencia ocorrencia = new Ocorrencia
                {
                    Local = local,
                    Data = data,
                    Descricao = descricao,
                    Status = "Aberto",
                    EmailUsuario = usuarioAutenticado.Email //Registra o dono do registro
                };

                //Validar as entradas da ocorrencia a ser registrada
                ocorrencia.ValidaEntradas();
                Db.Ocorrencias.Add(ocorrencia); //Adiciona a ocorrência ao banco de dados

                //Adiciona um log de evento
                Db.Eventos.Add(new Eventos
                {
                    DataEvento = DateTime.Now,
                    Acao = "Adicionar Ocorrência",
                    Descricao = $"Ocorrência adicionada no local: {local} com data: {data}."
                });

                Console.WriteLine("Ocorrência registrada com sucesso!");


            }

            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao registrar ocorrência: {ex.Message}");
            }
        }
        //Gera o ticket para a concessionária com base a última ocorrencia registrada pelo consumidor
        public void GerarTicket()
        {
            if (Db.Ocorrencias.Count == 0)
            {
                Console.WriteLine("Nenhuma ocorrência registrada.");
                return;
            }

            var ultimaOcorrencia = Db.Ocorrencias.Last();

            Ticket ticket = new Ticket
            {
                Id_ticket = new Random().Next(1, 1000), //Gera um ID aleatório para o ticket
                Id = ultimaOcorrencia.Id,
                Local = ultimaOcorrencia.Local,
                Data = ultimaOcorrencia.Data,
                Descricao = ultimaOcorrencia.Descricao,
                Status = "Em Análise",
                DataTicket = DateTime.Now,
                EmailUsuario = ultimaOcorrencia.EmailUsuario
            };

            ticket.CalculaPrioridade(); //Calcula a prioridade do ticket
            Db.Tickets.Add(ticket); //Adiciona o ticket ao banco de dados

            Db.Eventos.Add(new Eventos
            {
                DataEvento = DateTime.Now,
                Acao = "Gerar Ticket",
                Descricao = $"Ticket gerado com ID: {ticket.Id_ticket} para a ocorrência no local: {ticket.Local}."
            });

            Console.WriteLine($"Ticket gerado com sucesso! ID: {ticket.Id_ticket}, Prioridade: {ticket.Prioridade}");

        }

        //Lista de todos os chamados do usuário atual
        public void VisualizarRegistros()
        {
            var ocorrenciasUser = Db.Ocorrencias
                .Where(o => o.EmailUsuario == usuarioAutenticado.Email)
                .ToList();

            if (ocorrenciasUser.Count == 0)
            {
                Console.WriteLine("Você ainda não abriu nenhum chamado");
                return;
            }
            //Exibe a lista de ocorrencias registradas do usuário especifico
            foreach(var ocorrencia in ocorrenciasUser)
            {
                Console.WriteLine($"ID: {ocorrencia.Id} | Local: {ocorrencia.Local} | Data: {ocorrencia.Status}");
            }

        }

        //Exibe todos os chamados, apenas admin pode usar esse método
        public void VisualizarTodosChamados()
        {
            if (usuarioAutenticado.Role != "admin")
            {
                Console.WriteLine("Acesso negado. Apenas admins podem ver todos os chamados");
                return;
            }

            if(Db.Ocorrencias.Count == 0)
            {
                Console.WriteLine("Nenhuma ocorrência registrada");
                return;
            }

            foreach(var ocorrencia in Db.Ocorrencias)
            {
                Console.WriteLine($"ID: {ocorrencia.Id} | Local: {ocorrencia.Local} | Data: {ocorrencia.Status} | Usuário: {ocorrencia.EmailUsuario} | status: {ocorrencia.Status}\nDescrição: {ocorrencia.Descricao}");
            }
        }

        //Gera um relátorio e salva ele em um execel na pasta "Relatorios" na pasta raiz do projeto
        public void SalvaTicketsNoExcel()
        {
            if (usuarioAutenticado.Role != "admin")
            {
                Console.WriteLine("Apenas admins podem salvar todos os tickets registrados");
                return;
            }

            if (Db.Tickets.Count == 0)
            {
                Console.WriteLine("Nenhum ticket foi registrado ainda");
                return;
            }

            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                string pastaRaiz = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\"));
                string pastaRelatorio = Path.Combine(pastaRaiz, "Relatorios");

                if (!Directory.Exists(pastaRelatorio))
                {
                    Directory.CreateDirectory(pastaRelatorio);
                }

              
                string pathArquivo = Path.Combine(pastaRelatorio, $"tickets_{DateTime.Now:yyyyMMdd_HHmm}.xlsx");

                using var package = new ExcelPackage();

                var planilha = package.Workbook.Worksheets.Add("Tickets");

                //Header
                planilha.Cells[1, 1].Value = "ID ticket";
                planilha.Cells[1, 2].Value = "ID ocorrência";
                planilha.Cells[1, 3].Value = "Local";
                planilha.Cells[1, 4].Value = "Data ocorrencia";
                planilha.Cells[1, 5].Value = "Status";
                planilha.Cells[1, 6].Value = "Prioridade";
                planilha.Cells[1, 7].Value = "Descrição";
                planilha.Cells[1, 8].Value = "Usuario";

                int linhaInicio = 2;

                foreach(var ticket in Db.Tickets)
                {
                    var ocorrencia = Db.Ocorrencias.FirstOrDefault(o => o.Id == ticket.Id);
                    planilha.Cells[linhaInicio, 1].Value = ticket.Id_ticket;
                    planilha.Cells[linhaInicio, 2].Value = ticket.Id;
                    planilha.Cells[linhaInicio, 3].Value = ticket.Local;
                    planilha.Cells[linhaInicio, 4].Value = ticket.Data.ToString("dd/mm/yyyy");
                    planilha.Cells[linhaInicio, 5].Value = ticket.Status;
                    planilha.Cells[linhaInicio, 6].Value = ticket.Prioridade;
                    planilha.Cells[linhaInicio, 7].Value = ticket.Descricao;
                    planilha.Cells[linhaInicio, 8].Value = ticket.EmailUsuario ?? "Desconhecido";

                    linhaInicio++;
                }


                package.SaveAs(new FileInfo(pathArquivo));

                Console.WriteLine($"Relatório Excel salvo com sucesso em: {pathArquivo}");

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Erro ao exportar tickets: {ex.Message}");
            }
        }

    }
}
