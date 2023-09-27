namespace UI;
public static class RegisterServices {
    public static void ConfigureServices(this WebApplicationBuilder builder) {
        builder.Services.AddServerSideBlazor();
        builder.Services.AddRazorPages();

        builder.Services.AddSingleton<IDbConnection, DbConnection>();
        builder.Services.AddSingleton<IFriendRequestData, MongoFriendRequestData>();
        builder.Services.AddSingleton<IConversationData, MongoConversationData>();
        builder.Services.AddSingleton<IUserData, MongoUserData>();
    }
}
