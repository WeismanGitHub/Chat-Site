using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();
ConfigureServices.Configure(builder.Services);
var config = builder.Configuration;

string connectionId = "MongoProd";// Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "MongoDev" : "MongoProd";
var settings = builder.Configuration.GetSection("Settings").Get<Settings>()!;
settings.Database.ConnectionString = builder.Configuration.GetConnectionString(connectionId)!;
settings.Auth.SigningKey = builder.Configuration["SigningKey"]!;
builder.Services.AddAuthentication().AddJwtBearer(settings.Auth.SigningKey);

var app = builder.Build();

app.UseAuthentication()
    .UseHttpsRedirection()
    .UseAuthorization()
    .UseResponseCaching()
    .UseSwaggerGen()
    .UseFastEndpoints(c => c.Endpoints.RoutePrefix = "api");

await InitDatabase();


app.Run();

async Task InitDatabase() {
    BsonSerializer.RegisterSerializer(new ObjectSerializer(type =>
        ObjectSerializer.DefaultAllowedTypes(type) || type.Name!.EndsWith("Message"))
    );
    await DB.InitAsync(settings.Database.Name, MongoClientSettings.FromConnectionString(settings.Database.ConnectionString));
    await DB.MigrateAsync();
}
