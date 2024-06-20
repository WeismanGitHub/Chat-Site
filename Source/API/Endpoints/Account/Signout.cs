using FastEndpoints.Security;

namespace API.Endpoints.Account.Signout;

public sealed class Endpoint : EndpointWithoutRequest {
	public override void Configure() {
		Post("/Signout");
		Group<AccountGroup>();
		Version(1);
	}

	public override async Task HandleAsync(CancellationToken cancellationToken) {
		await CookieAuth.SignOutAsync();
	}
}
