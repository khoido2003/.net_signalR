using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace SignalRServer.Hubs
{
  public class ChatHub : Hub
  {
    public override Task OnConnectedAsync()
    {
      Console.WriteLine("=> Connection Established");

      Clients.Client(Context.ConnectionId).SendAsync("ReceivedConnId", Context.ConnectionId);

      return base.OnConnectedAsync();
    }

    public async Task SendMessageAsync(string message)
    {
      var routeOb = JsonConvert.DeserializeObject<dynamic>(message);

      string toClient = routeOb.To;

      Console.WriteLine("MessageReceived: " + Context.ConnectionId);

      if (toClient == string.Empty)
      {
        await Clients.All.SendAsync("ReceiveMessage", message);
      }
      else
      {
        await Clients.Client(toClient).SendAsync("ReceiveMessage", message);
      }
    }
  }
}