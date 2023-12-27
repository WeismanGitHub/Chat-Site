using API.Database.Entities;

using Microsoft.AspNetCore.SignalR;
namespace API;

public class ChatHub : Hub {
	public override async Task OnConnectedAsync() {
		// string conversationID = Context.GetHttpContext()?.Request.Query["conversationID"];
		// string accountID = Context.GetHttpContext()?.Request.Query["accountID"];

		// if (conversationID == null || accountID == null) {
		// 	return;
		// }

		// var user = await DB.Find<User>().MatchID(accountID).ExecuteSingleAsync();

		// if (user == null || !user.ConversationIDs.Contains(conversationID)) {
		// 	return;
		// }

		// await Groups.AddToGroupAsync(this.Context.ConnectionId, conversationID);
		// await Clients.Group(conversationID).SendAsync("UserJoined", accountID);
	}

	public override async Task OnDisconnectedAsync(Exception exception) {
		// var connectionGroups = await Groups.GetGroupsAsync(Context.ConnectionId);

		// // Handle the groups associated with the connection
		// foreach (var group in connectionGroups) {
		// 	// Do something with the group (e.g., log it, send it to the client)
		// 	await Clients.Caller.SendAsync("ReceiveGroup", group);
		// }
	}

	public async Task SendMessage(string message) {
		await Clients.All.SendAsync("ReceiveMessage", message);
		// await Clients.Group("convoID").SendAsync("ReceiveMessage", new { message });
	}
}
