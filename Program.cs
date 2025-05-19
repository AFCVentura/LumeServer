// Namespaces são basicamente o caminho dessa classe dentro do projeto, não precisa ser exatamente o mesmo caminho das pastas, mas é mais fácil adotar esse padrão.
using LumeServer.Data;
using LumeServer.Models.User;
using LumeServer.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace LumeServer
{
    // A classe Program é o entrypoint da aplicação, é aqui que configurações de serviços e afins são feitas.
    public class Program
    {
        // O método Main é o método que é chamado quando a aplicação é iniciada.
        public static void Main(string[] args)
        {
            // O builder é o objeto que builda a aplicação, ele é responsável por adicionar serviços, configurar a aplicação e afins.
            var builder = WebApplication.CreateBuilder(args);

            // Configuração de CORS
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            // Configura o de serialização JSON para ignorar ciclos de referência
            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                });

            // Aqui estamos dizendo que essa aplicação vai usar Controllers (porque tem como fazer sem eles também).
            builder.Services.AddControllers();

            // Carrega string de conexão do appsettings.json ou variáveis de ambiente
            var connectionString =
                builder.Configuration.GetConnectionString("CONNECTION_STRING");

            // Aqui estamos registrando o UserService com injeção de dependência.
            builder.Services.AddScoped<UserService>();

            // Registra o DbContext com injeção de dependência
            builder.Services.AddDbContext<LumeDataContext>(options =>
            {
                // Configura o serviço DataContext que será usado para a interação com o banco de dados.
                // O DataContext é uma classe que representa uma sessão com o banco de dados e permite a 
                // execução de consultas e operações de CRUD.

                options.UseMySql(
                    // Aqui, estamos dizendo ao DataContext para usar o MySQL como o provedor de banco de dados.
                    builder
                        .Configuration
                        // O método `Configuration` acessa as configurações da aplicação.
                        .GetConnectionString("CONNECTION_STRING"),
                    // Recupera a string de conexão com o banco de dados a partir do arquivo de configuração.
                    // Neste caso, a string é identificada pelo nome "CONNECTION_STRING".

                    ServerVersion
                        .AutoDetect(
                            // O método `AutoDetect` automaticamente detecta a versão do servidor MySQL
                            // para garantir que a aplicação se conecte corretamente à versão compatível.
                            builder
                                .Configuration
                                // Novamente, acessa a configuração da aplicação.
                                .GetConnectionString("CONNECTION_STRING")
                        // Obtém novamente a mesma string de conexão, que será usada para detectar a versão do MySQL.
                        )
                );
            });

            // Aqui estamos registrando o serviço de autenticação com JWT.
            builder.Services.AddAuthorization();

            // Aqui estamos registrando o serviço de autenticação com JWT.
            builder.Services
                .AddIdentityApiEndpoints<User>()
                .AddEntityFrameworkStores<LumeDataContext>();

            // Aqui estamos adicionando o serviço de documentação da API com Swagger, podemos deixar por enquanto.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Aqui estamos construindo a aplicação depois de fazer todas as configurações necessárias.
            var app = builder.Build();

            app.UseCors("AllowAll");

            // Aqui estamos aplicando as migrações pendentes do banco de dados.
            app.MapIdentityApi<User>();

            // Aqui nesse 'if' vai tudo que é necessário configurar apenas quando estamos em ambiente de desenvolvimento.
            if (app.Environment.IsDevelopment())
            {
                // Aqui são os dois métodos para fazer o Swagger funcionar.
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Sempre que alguém tentar acessar nosso servidor usando http://, ele vai ser redirecionado para https://
            app.UseHttpsRedirection();

            // Isso vai servir quando implementarmos o JWT.
            app.UseAuthorization();

            // Aqui a aplicação tá vendo todos os controllers e definindo qual a URL de cada um.
            app.MapControllers();

            // Aqui a aplicação finalmente executa depois de configurarmos tudo.
            app.Run();
        }
    }
}
