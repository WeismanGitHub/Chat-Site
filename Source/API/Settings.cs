namespace API;

public sealed class Settings {
    public DatabaseSettings Database { get; set; }
    public JWTAuthSettings Auth { get; set; }

    public class DatabaseSettings {
        public string Name { get; set; }
        public string MongoProd { get; set; }
        public string MongoDev { get; set; }
    }

    public class JWTAuthSettings {
        public int TokenValidityMinutes { get; set; }
    }
}
