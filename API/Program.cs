using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder();
var config = builder.Configuration;

string connectionId = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "MongoDev" : "MongoProd";
var settings = builder.Configuration.GetSection("Settings").Get<Settings>()!;
settings.Database.ConnectionString = builder.Configuration.GetConnectionString(connectionId)!;

var app = builder.Build();

app.UseFastEndpoints()
    .UseAuthentication()
    .UseHttpsRedirection()
    .UseAuthorization()
    .UseResponseCaching()
    .UseFastEndpoints(c => c.Serializer.Options.PropertyNamingPolicy = null);

await InitDatabase();

if (!builder.Environment.IsProduction()) {
    app.UseSwaggerGen();
}

app.Run();

async Task InitDatabase() {
    BsonSerializer.RegisterSerializer(new ObjectSerializer(type =>
        ObjectSerializer.DefaultAllowedTypes(type) || type.Name!.EndsWith("Message"))
    );
    await DB.InitAsync(settings.Database.ConnectionString);
    await DB.MigrateAsync();
}
