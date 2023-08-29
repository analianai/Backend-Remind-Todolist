using Backend.Models;
using Backend.Services.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class TasksController : Controller
    {
        private readonly TasksService tasksService;

        public TasksController(TasksService tasksService) {
            this.tasksService = tasksService;
        }

        //Listar tarefa pelo id da tarefa E ID USER ok
        /// <summary>
        /// Lista tarefa pelo id, se for do usuario logado
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpGet("{taskId}")]
        public IActionResult GetOneTaskBy(int taskId)
        {
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

                return Ok(tasksService.GetOneTaskBy(taskId, userId));
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        //Listar todas as tarefas do usuário logado OK
        /// <summary>
        /// Listar todas as tarefas do usuário logado
        /// </summary>
        [HttpGet("user")]
        public IActionResult GetOneTaskByUser()
        {
            try
            {
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

                return Ok(tasksService.GetOneTaskByUser(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        //Procurar pela informação do status referente ao usuario logado (id) 
        /// <summary>
        /// Procurar pelo status da tarefa do usuario logado
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet("status/{status}")]
        public IActionResult SearchStatusTaskByUser(string status)
        {
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

            return Ok(tasksService.SearchStatusTaskByUser(status, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Procurar pela informação do nome referente ao usuario logado (id) 
        /// <summary>
        /// Procurar pelo titulo da tarefa do usuario logado
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        [HttpGet("nome/{title}")]
        public IActionResult SearchNameTaskByUser(string title)
        {
            try
            {
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

                return Ok(tasksService.SearchNameTaskByUser(title, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Create quando mesmo Id do usuário e quando status for Planejado OK
        /// <summary>
        /// Cria tarefa para usuário logado
        /// </summary>
        /// <remarks>
        /// Sample request:
        /// 
        ///     POST/ Create
        ///     {
        ///             "title": "Alteração no User 12",
        ///             "description": "Remover funcionalidades extras",
        ///             "status": "Planejado",
        ///             "userId": 27
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateTastByUser(Tasks tasks)
        {
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

                tasksService.CreateTastByUser(tasks, userId);
                return Ok(tasks);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // Atualizar quando mesmo Id do usuário e quando titulo for diferente Ok
        /// <summary>
        /// Atualiza usuário logado com titulo diferente
        /// </summary>
        ///  /// <remarks>
        /// Sample request:
        /// 
        ///     PUT/ Update
        ///     {
        ///             "taskId": 13,
        ///             "title": "Alteração no User 12",
        ///             "description": "Remover funcionalidades extras",
        ///             "status": "Planejado",
        ///             "userId": 27
        ///     }
        ///     
        /// </remarks>
        /// <returns></returns>       
        [HttpPut("{taskId}")]
        public IActionResult UpdateTastByUser(int taskId, Tasks task)
        {
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

                tasksService.UpdateTastByUser(taskId, task, userId);
                return Ok(task);
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // Deleta quando usuário estiver autenticado e quando status for planejado Ok
        /// <summary>
        /// Deleta quando usuário estiver logado e status for planejado.
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns></returns>
        [HttpDelete("{taskId}")]
        public IActionResult DeleteTaskByUser(int taskId)
        {
            try
            {
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

                tasksService.DeleteTaskByUser(taskId, userId);
                return Ok("Tarefa deletada ID: "  + taskId);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
