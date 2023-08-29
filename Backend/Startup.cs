using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Backend
{
    public class Startup
    {
        public void ConfigureSwagger(WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(options => {

                // Configuração do documento Swagger
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Re.mind API",
                    Version = "v1",
                    Description = "Re.mind is a Kanban-like API for managing people's tasks with TDAH (Attention Deficit Hyperactivity Disorder)",
                    Contact = new OpenApiContact
                    {
                        Name = "Anália, Geismar e Jessica",
                        Url = new Uri("https://www.digitalhouse.com/br")
                    }
                });

                // Definição do esquema de segurança "Bearer"
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Por favor insira o token",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });

                // Adição do requisito de segurança "Bearer" para todas as operações
                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                new OpenApiSecurityScheme {
                    Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                Array.Empty<string>()
                }
            });
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
        }
    }
}

