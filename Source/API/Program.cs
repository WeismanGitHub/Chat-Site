using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using Microsoft.AspNetCore.Mvc;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);
ConfigureServices.Configure(builder);
var config = builder.Configuration;
var section = config.GetSection("Settings");
var settings = section.Get<Settings>()!;
var app = builder.Build();

app.MapFallbackToFile("/index.html");
app
	.UseAuthentication()
	.UseDefaultExceptionHandler()
	.UseAuthorization()
	.UseDefaultFiles()
	.UseStaticFiles()
	.UseFastEndpoints(config => {
		config.Endpoints.RoutePrefix = "API";

		config.Errors.ResponseBuilder = (failures, ctx, statusCode) => {
			return new ValidationProblemDetails(
				failures.GroupBy(f => f.PropertyName)
				.ToDictionary(
					keySelector: e => e.Key,
					elementSelector: e => e.Select(m => m.ErrorMessage).ToArray())) {
				Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
				Title = "One or more validation errors occurred.",
				Status = statusCode,
				Instance = ctx.Request.Path,
				Extensions = { { "traceId", ctx.TraceIdentifier }
					}
			};
		};
	})
	.UseHttpsRedirection()
	.UseResponseCaching()
	.UseSwaggerGen();

await InitDatabase();
app.Run();

async Task InitDatabase() {
	BsonSerializer.RegisterSerializer(new ObjectSerializer(type =>
		ObjectSerializer.DefaultAllowedTypes(type) || type.Name!.EndsWith("Message"))
	);

	var connectionString = settings.Database.MongoURI;
	await DB.InitAsync(settings.Database.Name, MongoClientSettings.FromConnectionString((connectionString)));

	await DB.Index<User>()
		.Key(u => u.Email, KeyType.Ascending)
		.Option(o => o.Unique = true)
		.CreateAsync();

	await DB.Index<FriendRequest>()
		.Key(fr => fr.RecipientID, KeyType.Ascending)
		.CreateAsync();

	await DB.MigrateAsync();
}

public partial class Program { }
