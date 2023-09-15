using System.ComponentModel.DataAnnotations;

namespace API.Models {
    public class User {
        public User(string name, string email, string password) {
            Name = name;
            Email = email;
            Password = password;
            CreatedTimestamp = DateTime.Now;
            ID = new Guid();
        }
        
        [Key]
        public Guid ID { get; set; }
        [Required, MaxLength(100), MinLength(1)]
        public string Name { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } // I know I need to hash passwords.
        [Required, Timestamp]
        public DateTime CreatedTimestamp { get; set; }
    }
}
