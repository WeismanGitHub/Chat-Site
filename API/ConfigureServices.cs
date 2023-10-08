using FastEndpoints;
using FastEndpoints.Swagger;

namespace API;

public static class ConfigureServices {
    public static void Configure(IServiceCollection services) {
        services.AddFastEndpoints()
            .SwaggerDocument()
            .AddResponseCaching();

        services.AddCors(options => {
            options.AddPolicy(name: "_myAllowSpecificOrigins", policy => {
                policy.WithOrigins("https://localhost:5173");
            });
        });

    }
}
