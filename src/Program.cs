using SteamWorkshopStats.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();
builder.Services.AddSingleton<DiscordService>();

var app = builder.Build();

// Middlewares
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();