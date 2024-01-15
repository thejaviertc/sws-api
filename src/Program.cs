using SteamWorkshopStats.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

builder.Services.AddHttpClient("SteamClient", client =>
{
    client.BaseAddress = new Uri("https://api.steampowered.com/");
});

builder.Services.AddHttpClient("DiscordClient", client =>
{
    client.BaseAddress = new Uri("https://discord.com/api/webhooks/");
});

builder.Services.AddSingleton<ISteamService, SteamService>();
builder.Services.AddSingleton<DiscordService>();

var app = builder.Build();

// Middlewares
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();