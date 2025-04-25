# LumeServer
Este repositório é focado no servidor do projeto Lume.

# Para executar o projeto, siga os passos abaixo:

## 1. Dependências

Instale o MySQL, o .NET 8.0 e o Visual Studio 2022.

## 2. Configuração do Banco de Dados

Verifique se existe um arquivo chamado **appsettings.Development.json**, se não houver, crie ele e adicione o seguinte código:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "CONNECTION_STRING": "server=localhost;userid=ventura;password=1234;database=Lume"
  }
}
```

Na parte da "CONNECTION_STRING", troque os campos server, userid , password e database pelos dados do seu banco de dados, não é necessário criar o banco de dados, o projeto já faz isso ao executar os próximos comandos.

## 3. Criando o banco de dados

Para criar o banco de dados e as tabelas automaticamente, é preciso da ferramenta **dotnet ef**, para instalá-la, execute o seguinte comando no terminal:

```bash
dotnet tool install --global dotnet-ef
```

Depois de instalada, execute o seguinte comando para criar o banco de dados e as tabelas:

```bash
dotnet ef database update
```

Se tudo der certo, serão exibidos comandos SQL e logs sobre a conexão e criação do banco.

## 4. Executando o projeto
Para executar o projeto, basta clicar no botão de play vazio ou no preenchido.

Será aberto um terminal que mostra os logs do servidor, e o servidor estará rodando numa porta específica.

Também será aberta uma guia do Swagger no navegador para testar as requisições.

É possível testar pelo arquivo LumeServer.http também, escrevendo a requisição desejada.

