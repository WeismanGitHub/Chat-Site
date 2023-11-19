using Microsoft.AspNetCore.SignalR;

namespace API;
public class ChatHub : Hub {
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ChatHub(IHttpContextAccessor httpContextAccessor) {
		_httpContextAccessor = httpContextAccessor;
	}

	private string GetAccountIDFromCookie() {
		// throw error if accountID is missing? idrk
		return _httpContextAccessor.HttpContext?.Request.Cookies[Claim.AccountID];
	}

	public async Task ConnectConvo(string convoID) {
		var accountID = GetAccountIDFromCookie();

		await Groups.AddToGroupAsync(Context.ConnectionId, convoID);
		await Clients.Group(convoID).SendAsync("UserConnected", new { UserID = accountID });
	}
	public async Task DisconnectConvo(string convoID) {
		var accountID = GetAccountIDFromCookie();

		await Clients.Group(convoID).SendAsync("UserDisconnected", new { UserID = accountID });
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, convoID);
	}

	public async Task SendMessage(string convoID, string message) {
		await Clients.Group(convoID).SendAsync("ReceiveMessage", new { message });
	}
}
