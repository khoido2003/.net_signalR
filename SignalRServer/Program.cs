using SignalRServer.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors();
builder.Services.AddSignalR();

var app = builder.Build();

app.UseCors(builder => builder
.WithOrigins("http://localhost:8082")
.AllowAnyHeader()
.AllowAnyMethod()
.AllowCredentials()
);

app.MapHub<ChatHub>("/chathub");

app.Run();
