using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;

namespace API;
public class ChatHub : Hub {
	public async Task JoinConvo(string convoID) {
		await Clients.Group(convoID).SendAsync("UserJoined", new { UserID = "testid" });
		await Groups.AddToGroupAsync(Context.ConnectionId, convoID);
	}

	public async Task LeaveConvo(string convoID) {
		await Clients.Group(convoID).SendAsync("UserLeft", new { UserID = "testid" });
		await Groups.RemoveFromGroupAsync(Context.ConnectionId, convoID);
	}
	
	public async Task SendMessage(string convoID, string message) {
		await Clients.Group(convoID).SendAsync("ReceiveMessage", new { message });
	}
}
