using Backend.Data;
using Backend.Models;
using System.Threading.Tasks;

namespace Backend.Services.Tasks
{
	public class TasksService
	{
		private readonly ApplicationDbContext context;
		private IConfiguration Configuration { get; }

		public TasksService(ApplicationDbContext context, IConfiguration configuration) {
			this.context = context;
			Configuration = configuration;
		}

		//Listar tarefa pelo id da tarefa referente ao usuario logado (id) OK
		public Models.Tasks? GetOneTaskBy(int taskId, int userId) {
			try
			{
				var task = context.Tasks.FirstOrDefault(t => t.TaskId == taskId && t.UserId == userId);
				if (task?.UserId != userId)
				{
					throw new Exception("ERRO - Tarefa não correspode ao Usuário");
				}
				return task;
			}
			catch (Exception error) {
				throw new Exception("Ocorreu um erro ao buscar a tarefa. Tente novamente mais tarde. " + error.Message);
			}
		}

		//Listar todas as tarefas do usuário logado OK
		public List<Models.Tasks> GetOneTaskByUser(int userId)
		{
			try 
			{
				var task = context.Tasks.Where(t => t.UserId == userId).ToList();
                if (task == null)
                {
                    throw new Exception($"Não foi encontrado nenhuma tarefa para esse usuário: {userId}");
                }
                return task;
			}
			catch (Exception error)
			{
				throw new Exception("Ocorreu um erro ao buscar a tarefa do Usuario. Tente novamente mais tarde. " + error.Message);
			}
		}

		//Procurar pela informação do status referente ao usuario logado (id) OK
		public virtual Models.Tasks SearchStatusTaskByUser(string status, int userId)
        {
            try
			{
                var task = context.Tasks.FirstOrDefault(t => t.Status == status && t.UserId == userId);
                if (task == null)
                {
                    throw new Exception($"Não foi encontrado nenhuma tarefa para esse status: {status}");
                }

                return task;
           
			}
			catch (Exception error)
			{
				throw new Exception("Ocorreu um erro ao buscar a tarefa por status. Tente novamente mais tarde. " + error.Message);
			}
		}

		///Procurar pela informação do nome referente ao usuario logado (id) OK
		public virtual Models.Tasks SearchNameTaskByUser(string title, int userId)
		{
			try
			{
                var task = context.Tasks.FirstOrDefault(t => t.Title == title && t.UserId == userId);
                if (task == null)
                {
                    throw new Exception($"Não foi encontrado nenhuma tarefa para esse ID: {userId}");
                }
				               
                return task;
			}
			catch (Exception error)
			{
				throw new Exception("Ocorreu um erro ao buscar a tarefa por nome. Tente novamente mais tarde. " + error.Message);
			}
		}

		// Create quando mesmo Id do usuário e quando status for Planejado OK
		public Models.Tasks CreateTastByUser(Models.Tasks tasks, int userId)
		{
			try
			{
				var owner = context.Users.FirstOrDefault(u => u.UserId == userId);
				if (owner != null && owner.UserId != tasks.UserId)
				{
					throw new Exception("Erro de Acesso");
				}
				else
				{
					if (tasks.Status != "Planejado")
					{
						throw new Exception("Erro do status");
					}
					context.Tasks.Add(tasks);
					context.SaveChanges();
					return tasks;
				}
			}
			catch (Exception error)
			{
				throw new Exception("Ocorreu um erro ao cadastrar uma tarefa, tente novamente mais tarde. " + error.Message);
			}
		}

		// Atualizar quando mesmo Id do usuário e quando titulo for diferente OK
		public virtual Models.Tasks UpdateTastByUser( int taskId, Models.Tasks task, int userId)
		{
			try 
			{
                var owner = context.Users.FirstOrDefault(u => u.UserId == userId);
                if (owner?.UserId != task.UserId)
                {
                    throw new Exception("Erro de Acesso");
                }
                
                var taskToUpdate = context.Tasks.FirstOrDefault(t => t.TaskId == taskId && t.Title != task.Title);
				if (taskToUpdate == null)
				{
					throw new Exception("Erro: Titulo igual ou tarefa não existe");
				}
				else
				{
					taskToUpdate.Title = task.Title;
					taskToUpdate.Description = task.Description;
					taskToUpdate.Status = task.Status;
					taskToUpdate.UserId = task.UserId;

					context.SaveChanges();

					return taskToUpdate;
				}
            }
			catch (Exception error) {
				throw new Exception("Ocorreu um erro ao atualizar o tarefa, tente novamente mais tarde. " + error.Message);
			}
		}

		// Deletar quando usuário estiver autenticado e quando status for planejado Ok
		public virtual Models.Tasks DeleteTaskByUser(int taskId, int UserId)
		{
			try
			{
                var tasks = context.Tasks.FirstOrDefault(t => t.TaskId == taskId && t.UserId == UserId);
				if (tasks?.Status != "Planejado")
				{
					throw new Exception("Status ou ID não permite deletar");
				}
				else
				{
					var taskToRemove = context.Tasks.Find(taskId);
					if (taskToRemove == null)
					{
						throw new Exception("ID da tarefa não existe");
					}
					context.Tasks.Remove(taskToRemove);
					context.SaveChanges();

					return taskToRemove;
				}
            }
			catch (Exception error)
			{
				throw new Exception("Ocorreu um erro ao excluir a tarefa, tente novamente mais tarde. " + error.Message);
			}
		}
	}
}