using Microsoft.AspNetCore.SignalR;
using Microsoft.Identity.Client;

namespace API;

public class ChatHub : Hub {
	public async Task SendMessage(string message) {
		await Clients.All.SendAsync("ReceiveMessage", message);
		// await Clients.Group("convoID").SendAsync("ReceiveMessage", new { message });
	}
}
