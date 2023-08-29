using Backend.Models;
using Backend.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UsersService usersService;

        public UsersController(UsersService usersService) {
            this.usersService = usersService;
        }

        //Listar Usuario pelo ID o que esta autenticado
        /// <summary>
        /// Informa sobre o usuário Logado
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public IActionResult GetOne() {
            try {
                // Obtém o usuário autenticado do contexto da solicitação
                var httpUser = HttpContext.User;

                // Obtém o valor da reivindicação (claim) de ID do usuário
                var userIdClaim = httpUser.FindFirst(ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    throw new Exception("Token inválido!");
                }

                // Converte o valor da reivindicação (claim) para um inteiro
                var userId = int.Parse(userIdClaim.Value);

                return Ok(usersService.GetOne(userId));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// Cria novo usuários sem Autenticação
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST/ Create
        ///     {
        ///         "name": "Analia",
        ///         "email": "analia.ninae@email.com",
        ///         "password": "senha123"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("create")]
        [AllowAnonymous]
        public IActionResult Create(Users user)
        {
            try {
                usersService.Create(user);
                return Ok(user);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Atualiza usuário logado
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     PUT/ Update
        ///     {
        ///         "userId": "",
        ///         "name": "Analia",
        ///         "email": "analia.ninae@email.com",
        ///         "password": "senha123"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPut]
        [Authorize]
        public IActionResult Update(Users user) {
            try {
                // Obtém o usuário autenticado do contexto da solicitação
                var httpUser = HttpContext.User;

                // Obtém o valor da reivindicação (claim) de ID do usuário
                var userIdClaim = httpUser.FindFirst(ClaimTypes.Name);

                if (userIdClaim == null)
                {
                    throw new Exception("Token inválido!");
                }

                // Converte o valor da reivindicação (claim) para um inteiro
                var userId = int.Parse(userIdClaim.Value);

                usersService.Update(userId, user);
                return Ok(user);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Gera Token de Autenticação
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST/ token
        ///     {
        ///         "email": "joao@email.com",
        ///         "password": "senha123"
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost("auth")]
        [AllowAnonymous]
        public IActionResult Auth(Users user) {
            try {
                return Ok(new {token = usersService.Auth(user) });
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }
    }
}