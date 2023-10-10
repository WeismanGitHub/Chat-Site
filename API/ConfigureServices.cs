using FastEndpoints.Swagger;

namespace API;

public static class ConfigureServices {
    public static void Configure(IServiceCollection services) {
        services.AddFastEndpoints()
            .AddAuthorization()
            .AddResponseCaching()
            .SwaggerDocument(swagger => {
                swagger.ShortSchemaNames = true;
                swagger.DocumentSettings = settings => {
                    settings.Title = "Chat Site API";
                    settings.Version = "v1";
                };
            });


        services.AddCors(options => {
            options.AddPolicy(name: "_myAllowSpecificOrigins", policy => {
                policy.WithOrigins("https://localhost:5173");
            });
        });

    }
}
