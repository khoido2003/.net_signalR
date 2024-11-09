namespace WebSocketServer.Middlewares
{

  public static class WSServerMiddlewareExtensions
  {
    public static IApplicationBuilder UseWebSocketServer(this IApplicationBuilder builder)
    {
      return builder.UseMiddleware<WSServerMiddleware>();
    }

    public static IServiceCollection AddWebSocketManager(this IServiceCollection services)
    {
      services.AddSingleton<WebSocketServerManager>();
      return services;
    }
  }
}