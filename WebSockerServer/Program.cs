using System.Net.WebSockets;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseWebSockets();
app.Use(async (context, next) =>
{
  if (context.WebSockets.IsWebSocketRequest)
  {
    WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
    Console.WriteLine("Websocket connected!");
  }
  else
  {
    await next();
  }
});

app.Run();
