using GS_CSDev.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GS_CSDev.Services
{

    //Classe responsável por autenticar usuários
    public static  class LoginService
    {
        public static bool Encerrou = false;
        //Usuários cadastrados manualmente e estáticos para exemplo
        public static readonly List<User> Users = new()
        {
            new User{  Email = "admin@admin.com", Senha="admin", Role="admin" },
            new User{  Email = "bot@bot.com", Senha="12345", Role="user" }

        };

        //Método que retorna o objeto do usuário autenticado
        public static User? Autenticar()
        {
            try
            {
                Console.WriteLine("LOGIN");
                Console.WriteLine("Digite 'Sair' para fechar o programa");

                Console.WriteLine("Email: ");
                string email = Console.ReadLine();

                if(email?.ToLower() == "sair") //Encerra o programa se digitar "Sair" no email
                {
                    Encerrou=true;
                    return null; //Encerra o programa
                }

                Console.WriteLine("Senha: ");
                string senha = Console.ReadLine();

                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
                {
                    Console.WriteLine("Email ou senha não pode estar vázios");
                    return null;

                }
                //Procura o usuário com as credenciais fornecidas
                var user = Users.FirstOrDefault(u => u.Email == email && u.Senha == senha);

                //Verifica se as credenciais estão certas ou não
                if (user != null)
                {
                    Console.WriteLine("Login realizado com sucesso");
                    return user;

                }
                else
                {
                    Console.WriteLine("Email ou senha estão incorretos, tente novamente!");
                    return null;

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Algo de errado aconteceu durante o login");
                return null;
            }
        }

    }
}
