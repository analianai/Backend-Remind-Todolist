using Backend.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend.Services.Users
{
	public class UsersService
	{
		private readonly ApplicationDbContext context;
		private IConfiguration Configuration { get; }

		public UsersService(ApplicationDbContext context, IConfiguration configuration)
		{
			this.context = context;
			Configuration = configuration;
		}

		// Listar usuário pelo ID Concluido
		public Models.Users? GetOne(int UsersId)
		{
			try
			{
				var user = context.Users.Find(UsersId);
				return user;
			}
			catch (Exception error)
			{
				throw new Exception("Ocorreu um erro ao buscar os usuário, tente novamente mais tarde. " + error.Message);
			}
		}

		// Cadastra Usuario com restrição de email
		public virtual Models.Users Create(Models.Users user)
		{
			try
			{
				var owner = context.Users.FirstOrDefault(u => u.Email == user.Email);
				if (owner?.Email == user.Email && owner != null)
				{
					throw new Exception("Erro de Acesso: Usuário existente ou email já Cadastrado");
				}
				else
				{
					context.Users.Add(user);
					context.SaveChanges();
					return user;
				}
            }
			catch (Exception error)
			{
				throw new Exception("Ocorreu um erro ao cadastrar o usuário, tente novamente mais tarde. " + error.Message);
			}
		}

        //Atualiza usuario logado
        public Models.Users Update(int userId, Models.Users user)
        {
            try
            {
                var userUpdate = context.Users.FirstOrDefault(u => u.UserId == userId);

                if (userUpdate == null)
                {
                    throw new Exception($"Não foi encontrado usuário para esse id: {userId}");
                }

                var userExistsEmail = context.Users.FirstOrDefault(u => u.UserId != userId && u.Email == user.Email);

                if (userExistsEmail != null)
                {
                    throw new Exception($"Já existe um usuário para esse e-mail: {user.Email}");
                }

                userUpdate.Name = user.Name;
                userUpdate.Email = user.Email;
                userUpdate.Password = user.Password;

                context.SaveChanges();

                return userUpdate;
            }
            catch (Exception error)
            {
                throw new Exception($"Ocorreu um erro ao atualizar o usuário. {error.Message}");
            }
        }

        //Atenticação de usuario
        public string Auth(Models.Users user)
		{
			var userExists = context.Users.FirstOrDefault(u => u.Email == user.Email && u.Password == user.Password);
			if (userExists == null)
			{
				throw new Exception("Credenciais inválidas!");
			}
			return GetToken(userExists);
		}

		// Gerador de Token JW
		public string GetToken(Models.Users user)
		{
			// Cria uma instância do JwtSecurityTokenHandler para manipular o token JWT
			var tokenHandler = new JwtSecurityTokenHandler();

			// Obtém a chave secreta do JWT da seção de configuração
			var jwtKey = Configuration.GetSection("JWT:Key").Value;

			// Converte a chave secreta do JWT de string para bytes
			var key = Encoding.ASCII.GetBytes(jwtKey);

			// Cria as reivindicações (claims) do token com o id do usuário
			var claims = new ClaimsIdentity(new Claim[] {
				new Claim(ClaimTypes.Name, user.UserId.ToString())
			});

			// Cria a descrição do token, incluindo as reivindicações, tempo de expiração e assinatura com a chave secreta
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = claims,
				Expires = DateTime.UtcNow.AddDays(1), // Define o tempo de expiração do token como 1 dia a partir do UTC atual
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			// Cria o token JWT com base na descrição do token
			var token = tokenHandler.CreateToken(tokenDescriptor);

			// Retorna o token JWT como string
			return tokenHandler.WriteToken(token);
		}
	}
}