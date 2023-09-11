namespace ChatAPI.Models;

public class User {
    public string ID { get; set; }
    public string Name { get; set; }
    public string Password { get; set; } // I know I need to hash it.
}
