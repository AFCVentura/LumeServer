// Namespaces são basicamente o caminho dessa classe dentro do projeto, não precisa ser exatamente o mesmo caminho das pastas, mas é mais fácil adotar esse padrão.
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

            // Aqui estamos dizendo que essa aplicação vai usar Controllers (porque tem como fazer sem eles também).
            builder.Services.AddControllers();

            // Aqui estamos adicionando o serviço de documentação da API com Swagger, podemos deixar por enquanto.
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Aqui estamos construindo a aplicação depois de fazer todas as configurações necessárias.
            var app = builder.Build();

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
