using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace API;

public class ChatHub : Hub {
	private readonly static ConcurrentDictionary<string, string> ConversationMap = new ConcurrentDictionary<string, string>();

	public override async Task OnConnectedAsync() {
		try {
			var httpContext = Context.GetHttpContext();

			if (httpContext == null || httpContext.User?.Identity?.IsAuthenticated == false) {
				throw new Exception("You must log in.");
			}

			string conversationID = Context.GetHttpContext()?.Request.Query["conversationID"];
			var accountID = httpContext?.User?.FindFirst("accountID")?.Value;

			if (conversationID == null || accountID == null) {
				throw new Exception("Missing conversation ID or account ID.");
			}

			if (!ConversationMap.TryAdd(Context.ConnectionId, conversationID)) {
				throw new Exception("An internal server error occurred.");
			};

			var user = await DB.Find<User>().MatchID(accountID).ExecuteSingleAsync();

			if (user == null) {
				throw new Exception("Could not find user in database.");
			} else if (!user.ConversationIDs.Contains(conversationID)) {
				throw new Exception("User not in this conversation.");
			}

			await Groups.AddToGroupAsync(this.Context.ConnectionId, conversationID);
			await Clients.Group(conversationID).SendAsync("UserJoined", accountID);
		} catch (Exception ex) {
			Clients.Caller.SendAsync("ReceiveError", ex.Message);
		}
	}

	public override async Task OnDisconnectedAsync(Exception exception) {
		try {
			var accountID = Context.GetHttpContext()?.User?.FindFirst("accountID")?.Value;
			var conversationID = ConversationMap[Context.ConnectionId];

			if (conversationID == null || accountID == null) {
				throw new Exception("Missing conversation ID or account ID.");
			}

			ConversationMap.Remove(Context.ConnectionId, out conversationID);

			await Clients.Group(conversationID).SendAsync("UserLeft", accountID);
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationID);
		} catch (Exception ex) {
			Clients.Caller.SendAsync("ReceiveError", ex.Message);
		}
	}

	public async Task SendMessage(string message) {
		try {
			var accountID = Context.GetHttpContext()?.User?.FindFirst("accountID")?.Value;
			var conversationID = ConversationMap[Context.ConnectionId];

			if (conversationID == null || accountID == null) {
				throw new Exception("Missing conversation ID or account ID.");
			}

			await Clients.All.SendAsync("ReceiveMessage", message);
			await Clients.Group("convoID").SendAsync("ReceiveMessage", new { message, accountID });
		} catch (Exception ex ) {
			Clients.Caller.SendAsync("ReceiveError", ex.Message);
		}
	}
}
