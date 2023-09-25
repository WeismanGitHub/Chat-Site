namespace UI;
public static class RegisterServices {
    public static void ConfigureServices(this WebApplicationBuilder builder) {
        builder.Services.AddServerSideBlazor();
        builder.Services.AddMemoryCache();
        builder.Services.AddRazorPages();
    }
}
