using Microsoft.Extensions.Options;
using FastEndpoints.Security;

namespace API.Endpoints.Account.Signout;

public sealed class Endpoint : EndpointWithoutRequest {
    public IOptions<Settings> Settings { get; set; } = null!;

    public override void Configure() {
        Post("/Signout");
        Group<AccountGroup>();
        Version(1);

        Summary(settings => {
            settings.Summary = "Delete the auth cookie, signing you out.";
        });
    }

    public override async Task HandleAsync(CancellationToken cancellationToken) {
        await CookieAuth.SignOutAsync();
    }
}
