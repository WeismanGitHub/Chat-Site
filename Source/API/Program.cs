using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using FastEndpoints.Swagger;
using API.Utilities;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
ConfigureServices.Configure(builder);
var config = builder.Configuration;
var section = config.GetSection("Settings");
var settings = section.Get<Settings>()!;
var app = builder.Build();

app.MapFallbackToFile("/index.html");
app.MapHub<ChatHub>("/chat");
app
	.UseDefaultExceptionHandler()
	.UseResponseCaching()
	.UseHttpsRedirection()
	.UseDefaultFiles()
	.UseStaticFiles()
	.UseAuthentication()
	.UseAuthorization()
	.UseFastEndpoints(config => config.Endpoints.RoutePrefix = "API")
	.UseSwaggerGen();

await InitDatabase();
app.Run();

async Task InitDatabase() {
	BsonSerializer.RegisterSerializer(new ObjectSerializer(type =>
		ObjectSerializer.DefaultAllowedTypes(type) || type.Name!.EndsWith("Message"))
	);

	var connectionString = settings.Database.MongoURI;
	await DB.InitAsync(settings.Database.Name, MongoClientSettings.FromConnectionString(connectionString));

	await DB.Index<User>()
		.Key(u => u.Name, KeyType.Ascending)
		.Option(o => o.Unique = true)
		.CreateAsync();

	await DB.Index<ChatRoom>()
	.Key(u => u.Name, KeyType.Ascending)
	.Option(o => o.Unique = true)
	.CreateAsync();

	await DB.MigrateAsync();
}

public partial class Program { }
