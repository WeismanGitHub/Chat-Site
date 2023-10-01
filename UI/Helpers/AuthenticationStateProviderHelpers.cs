using Microsoft.AspNetCore.Components.Authorization;

namespace UI.Helpers;

public static class AuthenticationStateProviderHelpers {
    public static async Task<User> GetUserFromAuth(
        this AuthenticationStateProvider provider,
        IUserData userData
    ) {
        var authState = await provider.GetAuthenticationStateAsync();
        var objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
        
        return await userData.GetUserFromAuthentication(objectId);
    }
}
