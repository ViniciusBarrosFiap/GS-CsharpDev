
# **SGO - Sistema de Gestão de Ocorrências**

O **SGO** é um sistema console desenvolvido em **C# com .NET 6**, focado no **monitoramento de ocorrências energéticas** associadas a possíveis **ameaças cibernéticas**. A aplicação permite que **usuários registrem falhas de energia** e gera automaticamente **tickets** para concessionárias.  
Administradores podem **visualizar todos os chamados** e **gerar relatórios em Excel** com os dados dos tickets.

---

## **Como Rodar o Projeto**

###  **Pré-requisitos**
- [.NET SDK 6.0+](https://dotnet.microsoft.com/download/dotnet/6.0)
- Visual Studio (ou terminal)
- Conexão com a internet para instalar pacotes NuGet

### **Instalar dependência**
No terminal do projeto:

```bash
dotnet add package EPPlus --version 6.2.5
```

###  **Executar o Projeto**
No terminal:

```bash
dotnet run
```

---

## **Funcionalidades Implementadas**

| Funcionalidade                   | Descrição                                                           |
| -------------------------------- | ------------------------------------------------------------------- |
| `LoginService.Autenticar()`      | Autenticação de usuários com diferentes perfis (`admin` e `user`)   |
| `RegistrarOcorrenciaComTicket()` | Registro de ocorrência + geração automática de ticket               |
| `VisualizarRegistros()`          | Permite ao consumidor visualizar apenas seus próprios chamados      |
| `VisualizarTodosChamados()`      | Exclusivo para admin: exibe todos os chamados registrados           |
| `ExportarTicketsParaExcel()`     | Exporta todos os tickets para uma planilha `.xlsx` com EPPlus       |
| `ValidaEntradas()`               | Validação de campos obrigatórios e tratamento de exceções           |
| `CalculaPrioridade()`            | Define a criticidade do ticket com base em palavras-chave via regex |
| `Log de eventos`                 | Todas as ações relevantes são registradas na lista `Db.Eventos`     |

---

## **Tecnologias Utilizadas**

-  [EPPlus] para geração de arquivos Excel
-  Programação orientada a objetos (POO)
-  Regex para análise da descrição de ocorrências
-  Log interno com classe `Eventos`
-  Geração de relatórios automáticos na pasta `Relatorios/`
-  Controle de acesso por tipo de usuário (`admin` ou `user`)

---

##  **Usuários Pré-cadastrados**

| Email                                     | Senha | Role  |
| ----------------------------------------- | ----- | ----- |
| [admin@admin.com](mailto:admin@admin.com) | Admin | admin |
| [bot@bot.com](mailto:bot@bot.com)         | 12345 | user  |

---

##  **Relatórios Excel**

- A planilha é salva automaticamente na pasta `Relatorios/`, localizada na raiz do projeto.
- O nome do arquivo segue o padrão:

```
Tickets_yyyyMMdd_HHmmss.xlsx
```

- Cada relatório inclui os seguintes dados:
  - ID do Ticket
  - ID da Ocorrência
  - Local
  - Descrição
  - Status
  - Prioridade
  - Data do Ticket
  - E-mail do Usuário que abriu a ocorrência

---

> Esse projeto simula um sistema de gestão para concessionárias de energia com base em falhas que podem ter origem em **ataques cibernéticos**, demonstrando um uso prático de **registro inteligente, alerta automático e geração de relatórios operacionais.**
