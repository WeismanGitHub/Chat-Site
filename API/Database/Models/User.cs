using System.ComponentModel.DataAnnotations;

namespace API.Models {
    public class User {
        [Key]
        public int ID { get; set; }
        [Required, MaxLength(100), MinLength(1)]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } // I know I need to hash passwords.
        [Required]
        public virtual ICollection<User> Friends { get; set;}
        [Required, Timestamp]
        public DateTime CreatedTimestamp { get; set; }
    }
}
