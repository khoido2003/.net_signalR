namespace WebSocketServer.Middlewares
{

  public static class WSServerMiddlewareExtensions
  {
    public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<WSServerMiddleware>();
    }

  }
}