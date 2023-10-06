using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web.UI;
using Microsoft.Identity.Web;
using System.Drawing;

namespace UI;
public static class RegisterServices {
    public static void ConfigureServices(this WebApplicationBuilder builder) {
        builder.Services.AddServerSideBlazor();
        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews().AddMicrosoftIdentityUI();

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));

        string env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        string connectionId = env == "Development" ? "MongoDev" : "MongoProd";

        builder.Services.AddSingleton<IDbConnection, DbConnection>(_ => new DbConnection(builder.Configuration, connectionId));
        builder.Services.AddSingleton<IFriendRequestData, MongoFriendRequestData>();
        builder.Services.AddSingleton<IConversationData, MongoConversationData>();
        builder.Services.AddSingleton<IUserData, MongoUserData>();
    }
}
