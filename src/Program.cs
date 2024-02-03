using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using SteamWorkshopStats.Middlewares;
using SteamWorkshopStats.Services;

var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddRateLimiter(rateLimiterOptions =>
	rateLimiterOptions.AddFixedWindowLimiter(
		policyName: "fixed",
		options =>
		{
			options.PermitLimit = 50;
			options.Window = TimeSpan.FromMinutes(20);
			options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
			options.QueueLimit = 5;
		}
	)
);

builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(policy =>
	{
		policy.WithOrigins("https://thejaviertc.github.io").AllowAnyMethod().AllowAnyHeader();
	});
});

builder.Services.AddControllers();

builder.Services.AddHttpClient(
	"SteamClient",
	client =>
	{
		client.BaseAddress = new Uri("https://api.steampowered.com/");
	}
);

builder.Services.AddHttpClient(
	"DiscordClient",
	client =>
	{
		client.BaseAddress = new Uri("https://discord.com/api/webhooks/");
	}
);

builder.Services.AddSingleton<ISteamService, SteamService>();
builder.Services.AddSingleton<IDiscordService, DiscordService>();

var app = builder.Build();

// Middlewares
if (app.Environment.IsDevelopment())
	app.UseDeveloperExceptionPage();
else
{
	app.UseHsts();
	app.UseMiddleware<QueryLoggerMiddleware>();
}

app.UseMiddleware<ErrorLoggerMiddleware>();

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthorization();
app.UseRateLimiter();

app.MapControllers();

app.Run();
