using Backend;

var builder = WebApplication.CreateBuilder(args);
//Aplica a classe configure das configura��es e JWT
Configure configure = new Configure();
//Aplica a classe Startup com a documenta��o do Swagger
Startup startup = new Startup();

// Configura��o do Cors
configure.ConfigureCors(builder);

// Deploy no IIS
configure.ConfigureIIS(builder);

// Configura��es do contexto de banco de dados usando o provedor SQL Server
configure.ConfigureDB(builder);

// Configura��es de autentica��o JWT
configure.ConfigureJWT(builder);

// Configura��es do Swagger
startup.ConfigureSwagger(builder);

// Configura��es de autoriza��o
builder.Services.AddAuthorization();

// Adiciona a configura��o de controladores ao servi�o
builder.Services.AddControllers();

// Adiciona os servi�os de escopo para inje��o de depend�ncia
configure.ConfigureScoped(builder);

// Constr�i a aplica��o
var app = builder.Build();

// Habilita o CORS usando a pol�tica "AllowAllOrigins"
app.UseCors("AllowAllOrigins");

// Abre a pagina do swagger
app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Re.mind API v1");
    options.RoutePrefix = "";//string.Empty
});

app.UseAuthentication(); // Adiciona a autentica��o � pipeline da aplica��o
app.UseAuthorization(); // Adiciona a autoriza��o � pipeline da aplica��o

app.MapControllers(); // Mapeia os controladores da aplica��o

app.Run(); // Executa a aplica��o