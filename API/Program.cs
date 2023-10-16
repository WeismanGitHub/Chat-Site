using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();
ConfigureServices.Configure(builder.Services);
var config = builder.Configuration;

string connectionId = "MongoProd";// Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "MongoDev" : "MongoProd";
var section = config.GetSection("Settings");
var settings = section.Get<Settings>()!;
settings.Database.ConnectionString = config.GetConnectionString(connectionId)!;

builder.Services
    .Configure<Settings>(section)
    .AddAuthentication()
    .AddJwtBearer(settings.Auth.SigningKey);

var app = builder.Build();

app.UseAuthentication()
    .UseDefaultExceptionHandler()
    .UseFastEndpoints(c => c.Endpoints.RoutePrefix = "API")
    .UseHttpsRedirection()
    .UseAuthorization()
    .UseResponseCaching()
    .UseSwaggerGen();

await InitDatabase();
app.Run();

async Task InitDatabase() {
    BsonSerializer.RegisterSerializer(new ObjectSerializer(type =>
        ObjectSerializer.DefaultAllowedTypes(type) || type.Name!.EndsWith("Message"))
    );

    await DB.InitAsync(settings.Database.Name, MongoClientSettings.FromConnectionString(settings.Database.ConnectionString));

    await DB.Index<User>()
        .Key(u => u.Email, KeyType.Ascending)
        .Option(o => o.Unique = true)
        .CreateAsync();

    await DB.Index<FriendRequest>()
        .Key(fr => fr.RecipientId, KeyType.Ascending)
        .CreateAsync();

    await DB.MigrateAsync();
}
