using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    public class Tasks
    {
        [Key]
        public int TaskId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Status { get; set; }
        [ForeignKey("UserId")]
        public int UserId { get; set; }
        public Users? User { get; set; }
    }
}
