using Backend.Data;
using Backend.Services.Tasks;
using Backend.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Backend
{
    public class Configure
    {
        //libera todos os IP para acessar a API
        public void ConfigureCors(WebApplicationBuilder builder)
        {
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
            });
        }
        public void ConfigureIIS(WebApplicationBuilder builder)
        {
            builder.Services.Configure<IISServerOptions>(options =>
            {
                options.AutomaticAuthentication = false;
            });
        }

        public void ConfigureDB(WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
        }

        public void ConfigureScoped(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<UsersService>();
            builder.Services.AddScoped<TasksService>();
        }

        public void ConfigureJWT(WebApplicationBuilder builder)
        {
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, // Não valida a audiência do token
                    ValidateIssuer = false, // Não valida o emissor do token
                    ValidateLifetime = true, // Valida a expiração do token
                    ValidateIssuerSigningKey = true, // Valida a chave de assinatura do token
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"])) // Chave de assinatura do token
                };
            });
        }
    }
}
