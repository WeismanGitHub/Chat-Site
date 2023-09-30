using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;

namespace UI;
public static class RegisterServices {
    public static void ConfigureServices(this WebApplicationBuilder builder) {
        builder.Services.AddServerSideBlazor();
        builder.Services.AddRazorPages();

        builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
            .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureADB2C"));

        builder.Services.AddSingleton<IDbConnection, DbConnection>();
        builder.Services.AddSingleton<IFriendRequestData, MongoFriendRequestData>();
        builder.Services.AddSingleton<IConversationData, MongoConversationData>();
        builder.Services.AddSingleton<IUserData, MongoUserData>();
    }
}
