using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace WebSocketServer.Middlewares
{
  public class WebSocketServerManager
  {
    private ConcurrentDictionary<string, WebSocket> _socket = new ConcurrentDictionary<string, WebSocket>();

    public ConcurrentDictionary<string, WebSocket> GetAllSocket()
    {
      return _socket;
    }


    public string AddSocket(WebSocket socket)
    {
      string ConnId = Guid.NewGuid().ToString();
      _socket.TryAdd(ConnId, socket);

      Console.WriteLine("Connection Added: " + ConnId); ;

      return ConnId;

    }
  }
}