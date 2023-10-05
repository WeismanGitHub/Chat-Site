using Library;

using Microsoft.AspNetCore.Rewrite;
using UI;

var builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();
var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    int seed = int.Parse(builder.Configuration["DataGenerationSeed"]);
    var dataGenerator = new DataGenerator(seed);

    if (dataGenerator.CollectionsAreEmpty()) {
        await dataGenerator.ReplaceDatabaseWithBogus();
    }
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
