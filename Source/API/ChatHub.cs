using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;

namespace API;

public class Member {
	public required string Id { get; set; }
	public required string Name { get; set; }
}

public class ChatHub : Hub {
	private readonly static ConcurrentDictionary<string, string> ChatRoomMap = new ConcurrentDictionary<string, string>();

	public override async Task OnConnectedAsync() {
		try {
			var httpContext = Context.GetHttpContext();

			if (httpContext == null || httpContext.User?.Identity?.IsAuthenticated == false) {
				throw new Exception("You must log in.");
			}

			string? chatId = Context.GetHttpContext()?.Request.Query["chatRoomID"];
			var accountID = httpContext?.User?.FindFirst("accountID")?.Value;

			if (chatId == null || accountID == null) {
				throw new Exception("Missing chat room ID or account ID.");
			}

			if (!ChatRoomMap.TryAdd(Context.ConnectionId, chatId)) {
				throw new Exception("An internal server error occurred.");
			};

			var user = await DB.Find<User>().MatchID(accountID).ExecuteSingleAsync();

			if (user == null) {
				throw new Exception("Could not find user in database.");
			} else if (!user.ChatRoomIDs.Contains(chatId)) {
				throw new Exception("User is not in this chat room.");
			}

			await Groups.AddToGroupAsync(this.Context.ConnectionId, chatId);
			await Clients.Group(chatId).SendAsync("UserConnected", accountID);
		} catch (Exception ex) {
			await Clients.Caller.SendAsync("ReceiveError", ex.Message);
		}
	}

	public override async Task OnDisconnectedAsync(Exception exception) {
		try {
			var accountID = Context.GetHttpContext()?.User?.FindFirst("accountID")?.Value;
			var chatID = ChatRoomMap[Context.ConnectionId];

			if (chatID == null || accountID == null) {
				throw new Exception("Missing chat room ID or account ID.");
			}

			ChatRoomMap.Remove(Context.ConnectionId, out chatID);

			await Clients.Group(chatID).SendAsync("UserDisconnected", accountID);
			await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatID);
		} catch (Exception ex) {
			await Clients.Caller.SendAsync("ReceiveError", ex.Message);
		}
	}

	public async Task SendMessage(string message) {
		try {
			if (message.Length > 1000) {
				throw new Exception("Message is too long.");
			}

			var accountID = Context.GetHttpContext()?.User?.FindFirst("accountID")?.Value;
			var chatID = ChatRoomMap[Context.ConnectionId];

			if (chatID == null || accountID == null) {
				throw new Exception("Missing chat room ID or account ID.");
			}

			await Clients.Group(chatID).SendAsync("ReceiveMessage", new { message, accountID });
		} catch (Exception ex ) {
			await Clients.Caller.SendAsync("ReceiveError", ex.Message);
		}
	}

	public async Task UserJoined(string chatID, Member member) {
		try {
			await Clients.Group(chatID).SendAsync("UserJoined", member);
		} catch (Exception ex) {
			await Clients.Caller.SendAsync("ReceiveError", ex.Message);
		}
	}

	public async Task UserLeft(string chatID, string userID) {
		try {
			await Clients.Group(chatID).SendAsync("UserLeft", new { userID });
		} catch (Exception ex) {
			await Clients.Caller.SendAsync("ReceiveError", ex.Message);
		}
	}
}
