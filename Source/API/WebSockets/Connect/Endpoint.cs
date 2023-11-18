using System.Net.WebSockets;
using System.Text;

namespace API.WebSockets.Connect;

public sealed class WebSocketEndpoint : Endpoint<ConnectionRequest> {
    public override void Configure() {
        Verbs("CONNECT");
		Routes("/Connect");
        Version(1);
    }

	public override async Task HandleAsync(ConnectionRequest req, CancellationToken cancellationToken) {
		if (!HttpContext.WebSockets.IsWebSocketRequest) {
			ThrowError("Must be web socket request", 400);
		}
		
		using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

		while (webSocket.State == WebSocketState.Open) {
			var message = Encoding.UTF8.GetBytes("Hello World!");
			var arraySegment = new ArraySegment<byte>(message, 0, message.Length);

			await webSocket.SendAsync(arraySegment, WebSocketMessageType.Text, true, cancellationToken);

		}
	}
}
