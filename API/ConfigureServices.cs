using FastEndpoints.Security;
using FastEndpoints.Swagger;

namespace API;

public static class ConfigureServices {
    public static void Configure(WebApplicationBuilder builder) {
        var services = builder.Services;
        var config = builder.Configuration;

        var section = config.GetSection("Settings");
        var settings = section.Get<Settings>()!;

        builder.Services
            .Configure<Settings>(section)
            .AddFastEndpoints()
            .AddCookieAuth(validFor: TimeSpan.FromMinutes(settings.Auth.TokenValidityMinutes))
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
