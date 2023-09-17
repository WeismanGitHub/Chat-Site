using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using NuGet.Packaging.Signing;

namespace API.Models {
    public class User {
        [Key]
        public Guid ID { get; set; } = new Guid();
        [Required, MaxLength(100), MinLength(1)]
        public string Name { get; set; }
        [Required, EmailAddress, Index(nameof(Email), IsUnique = true)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } // I know I need to hash passwords.
        //[Timestamp]
        //public Timestamp CreatedTimestamp { get; set; }
    }

    public class UserDTO {
        [Required, MaxLength(100), MinLength(1)]
        public string Name { get; set; }
        [Required, EmailAddress, Index(nameof(Email), IsUnique = true)]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; } // I know I need to hash passwords.
    }
}
