using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using WebSocketServer.Middlewares;
using Newtonsoft.Json;

namespace WebSocketServer.Networks
{
  public class WebSocketHandler
  {
    private readonly WebSocketServerManager
     _manager;

    public WebSocketHandler(WebSocketServerManager manager)
    {
      _manager = manager;
    }

    public async Task HandleAsync(HttpContext context)
    {
      WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

      string ConnId = _manager.AddSocket(webSocket);
      await SendConnIdAsync(webSocket, ConnId);

      Console.WriteLine("WebSocket connected");

      await ReceiveMessageAsync(webSocket, async (result, buffer) =>
      {
        if (result.MessageType == WebSocketMessageType.Text)
        {
          Console.WriteLine("Message received");
          Console.WriteLine($"Message: {Encoding.UTF8.GetString(buffer, 0, result.Count)}");

          await RouteJsonMessageAsync(Encoding.UTF8.GetString(buffer, 0, result.Count));
          return;
        }
        else if (result.MessageType == WebSocketMessageType.Close)
        {
          string id = _manager.GetAllSocket().FirstOrDefault(s => s.Value == webSocket).Key;
          Console.WriteLine("Received close message");

          _manager.GetAllSocket().TryRemove(id, out WebSocket sock);
          await sock.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
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

    private async Task SendConnIdAsync(WebSocket socket, string ConnId)
    {
      var buffer = Encoding.UTF8.GetBytes("ConnId: " + ConnId);

      await socket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);
    }

    public async Task RouteJsonMessageAsync(string message)
    {
      var routeOb = JsonConvert.DeserializeObject<dynamic>(
      message
      );

      if (Guid.TryParse(routeOb.To.ToString(), out Guid guiOutput))
      {
        Console.WriteLine("TargetId");
        var sock = _manager.GetAllSocket().FirstOrDefault(s => s.Key == routeOb.To.ToString());

        if (sock.Value != null)
        {
          if (sock.Value.State == WebSocketState.Open)
          {
            await sock.Value.SendAsync(Encoding.UTF8.GetBytes(routeOb.Message.ToString()), WebSocketMessageType.Text, true, CancellationToken.None);
          }
          else
          {
            Console.WriteLine("Invalid recipient");
          }
        }
      }
      else
      {
        Console.WriteLine("Broadcast");
        foreach (var sock in _manager.GetAllSocket())
        {
          if (sock.Value.State == WebSocketState.Open)
          {
            await sock.Value.SendAsync(Encoding.UTF8.GetBytes(routeOb.Message.ToString()), WebSocketMessageType.Text, true, CancellationToken.None);
          }
        }
      }

    }
  }
}