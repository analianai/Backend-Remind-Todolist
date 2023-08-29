using Backend;

var builder = WebApplication.CreateBuilder(args);
//Aplica a classe configure das configurações e JWT
Configure configure = new Configure();
//Aplica a classe Startup com a documentação do Swagger
Startup startup = new Startup();

// Configuração do Cors
configure.ConfigureCors(builder);

// Deploy no IIS
configure.ConfigureIIS(builder);

// Configurações do contexto de banco de dados usando o provedor SQL Server
configure.ConfigureDB(builder);

// Configurações de autenticação JWT
configure.ConfigureJWT(builder);

// Configurações do Swagger
startup.ConfigureSwagger(builder);

// Configurações de autorização
builder.Services.AddAuthorization();

// Adiciona a configuração de controladores ao serviço
builder.Services.AddControllers();

// Adiciona os serviços de escopo para injeção de dependência
configure.ConfigureScoped(builder);

// Constrói a aplicação
var app = builder.Build();

// Habilita o CORS usando a política "AllowAllOrigins"
app.UseCors("AllowAllOrigins");

// Abre a pagina do swagger
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Re.mind API v1");
    options.RoutePrefix = "";//string.Empty
});

app.UseAuthentication(); // Adiciona a autenticação à pipeline da aplicação
app.UseAuthorization(); // Adiciona a autorização à pipeline da aplicação

app.MapControllers(); // Mapeia os controladores da aplicação

app.Run(); // Executa a aplicação