using SteamWorkshopStats.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

builder.Services.AddHttpClient("SteamClient", client =>
{
    client.BaseAddress = new Uri("https://api.steampowered.com/");
});

builder.Services.AddSingleton<SteamService>();

var app = builder.Build();

// Middlewares
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();