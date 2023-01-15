using SimpleChat.Hubs;
using SimpleChat.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddSingleton<IChatService, ChatService>();

builder.Services.AddSignalR();

var app = builder.Build();

app.MapHub<UserChatHub>("/chat-hub");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
