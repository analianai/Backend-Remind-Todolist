using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Users
    {
        [Key]
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
