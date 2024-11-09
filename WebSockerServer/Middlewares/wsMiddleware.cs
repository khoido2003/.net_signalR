
using WebSocketServer.Networks;

namespace WebSocketServer.Middlewares
{
  public class WSServerMiddleware
  {

    private readonly RequestDelegate _next;
    private readonly WebSocketHandler _webSocketHandler;

    public WSServerMiddleware(RequestDelegate next, WebSocketHandler webSocketHandler)
    {
      _next = next;
      _webSocketHandler = webSocketHandler;
    }

    public async Task InvokeAsync(HttpContext context)
    {
      if (context.WebSockets.IsWebSocketRequest)
      {

        // WriteRequestParams(context);
        await _webSocketHandler.HandleAsync(context);
      }
      else
      {
        Console.WriteLine("Hello from the 2rd request delegate");
        await _next(context);
      }
    }



  }
}