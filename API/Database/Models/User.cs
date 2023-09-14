﻿namespace API.Models {
    public class User {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public virtual ICollection<User> Friends { get; set;}
        public DateTime CreatedTimestamp { get; set; }
    }
}
