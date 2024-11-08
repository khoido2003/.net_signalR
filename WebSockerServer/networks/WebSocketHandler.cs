using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;

namespace WebSocketServer.Networks
{
  public class WebSocketHandler
  {
    public async Task HandleAsync(HttpContext context)
    {
      WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

      Console.WriteLine("WebSocket connected");

      await ReceiveMessageAsync(webSocket, async (result, buffer) =>
      {
        if (result.MessageType == WebSocketMessageType.Text)
        {
          Console.WriteLine("Message received");
          Console.WriteLine($"Message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");
          return;
        }
        else if (result.MessageType == WebSocketMessageType.Close)
        {
          Console.WriteLine("Received close message");
          return;
        }
      });
    }

    public async Task ReceiveMessageAsync(WebSocket webSocket, Action<WebSocketReceiveResult, byte[]> handleMessage)
    {
      var buffer = new byte[1024 * 4];

      while (webSocket.State == WebSocketState.Open)
      {
        var result = await webSocket.ReceiveAsync(buffer: new ArraySegment<byte>(buffer), cancellationToken: CancellationToken.None);

        handleMessage(result, buffer);
      }
    }
  }
}