
using GS_CSDev.Models;
using GS_CSDev.Services;

class Program
{
    static void Main(string[] args)
    {
        Console.Title = "SGO - Sistema de gestão de Ocorrências";
        bool menuFuncionando = true;

        while (menuFuncionando)
        {
            //Login
            User? usuarioAutenticado = LoginService.Autenticar();
            while (usuarioAutenticado == null)
            {
                usuarioAutenticado = LoginService.Autenticar();
                if (usuarioAutenticado == null)
                {
                    Console.WriteLine("Ocorreu uma falha no login.");
                    return;
                }


                if(usuarioAutenticado == null && LoginService.Encerrou)
                {
                    return;
                }
                
            }

            //Instanciando o método que lida com as ocorrências
            //Parâmetro: Recebe o usuário que foi logado
            var gestao = new GerenciaOcorrencia(usuarioAutenticado);

            bool executa = true;

            while(executa)
            {
                //INICIO
                Console.Clear();

                Console.WriteLine("SGO - Sistema de gestão de Ocorrências"); //Boas vindas
                Console.WriteLine($"Bem-vindo, {usuarioAutenticado.Email}({usuarioAutenticado.Role})");

                //Menu para Admins
                if (usuarioAutenticado.Role == "admin")
                {
                    Console.WriteLine("1 - visualizar todos os chamados registrados");
                    Console.WriteLine("2 - Exportar tickets para excel");
                    Console.WriteLine("0 - Trocar de conta");
                    Console.WriteLine("Escolha uma opção: ");
                    string? opcao = Console.ReadLine();

                    switch(opcao)
                    {
                        case "1":
                            gestao.VisualizarTodosChamados(); //Método permitido apenas para admin, lista todos os chamados
                            break;
                        case "2":
                            gestao.SalvaTicketsNoExcel(); //Método permitido apenas para admin, lista todos os chamados
                            break;
                        case "0":
                            executa = false; //Volta para a página de login
                            break;
                        default:
                            Console.WriteLine("Opção inválida! Tente novamente");
                            break;
                    }

                }
                else // Menu para usuário normal
                {
                    Console.WriteLine("1 - Registrar novo chamado");
                    Console.WriteLine("2 - Visualizar meus chamados");
                    Console.WriteLine("0 - Trocar de conta");
                    Console.WriteLine("Escolha uma opção: ");

                    string? opcao = Console.ReadLine();

                    switch (opcao)
                    {
                        case "1":
                            gestao.RegistraOcorrenciaTicket();
                            break;
                        case "2":
                            gestao.VisualizarRegistros(); //Permite visualizar os registros abertos apenas por aquele usuário
                            break;
                        case "0":
                            executa = false; //Volta para a tela de login
                            break;
                        default:
                            Console.WriteLine("Opção Inválida");
                            break;
                    }
                }
                //FINAL

                Console.WriteLine("\nPressione qualquer tecla para continuar...");
                Console.ReadKey();

            }

        }
    }
}