using FastEndpoints.Swagger;

namespace API;

public static class ConfigureServices {
    public static void Configure(IServiceCollection services) {
        services.AddFastEndpoints()
            .AddAuthorization()
            .SwaggerDocument(options => {
                options.MaxEndpointVersion = 1;
                options.ShortSchemaNames = true;
                options.DocumentSettings = settings => {
                    settings.Title = "Chat Site API v1";
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
