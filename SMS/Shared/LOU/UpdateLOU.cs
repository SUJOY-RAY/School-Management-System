using SMS.Models;
using System.ComponentModel.DataAnnotations;

namespace SMS.Shared.LOU
{
    public class UpdateLOU
    {
        public int Id { get; set; }   

        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public Role Role { get; set; }
    }
}
