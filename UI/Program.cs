using Library;

using Microsoft.AspNetCore.Rewrite;
using UI;

string connectionId = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? "MongoDev" : "MongoProd";
var builder = WebApplication.CreateBuilder(args);
builder.Configuration["MongoURI"] = builder.Configuration.GetConnectionString(connectionId);
builder.ConfigureServices();
var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    int seed = int.Parse(builder.Configuration["DataGenerationSeed"]);
    var db = new DbConnection(builder.Configuration);
    var dataGenerator = new DataGenerator(db, seed);
    dataGenerator.PopulateWithBogus();
} else {
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();

app.UseRewriter(new RewriteOptions().Add(
    context => {
        if (context.HttpContext.Request.Path == "/MicrosoftIdentity/Account/SignedOut") {
            context.HttpContext.Response.Redirect("/");
        }
    }
));

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
