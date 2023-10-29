namespace API;

public sealed class Settings {
    public DatabaseSettings Database { get; set; }
    public JWTAuthSettings Auth { get; set; }

    public class DatabaseSettings {
        public string MongoURI { get; set; }
        public string Name { get; set; }
    }

    public class JWTAuthSettings {
        public int TokenValidityMinutes { get; set; }
    }
}
