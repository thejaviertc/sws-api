using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using SteamWorkshopStats.Middlewares;
using SteamWorkshopStats.Services;

namespace SteamWorkshopStats;

public class Program
{
	private static WebApplicationBuilder? _builder;

	public static void Main(string[] args)
	{
		_builder = WebApplication.CreateBuilder(args);

		ConfigureServices();
		ConfigureHttpClients();

		_builder.Services.AddSingleton<ISteamService, SteamService>();
		_builder.Services.AddSingleton<IDiscordService, DiscordService>();

		WebApplication app = _builder.Build();

		// Middlewares
		if (app.Environment.IsDevelopment())
			app.UseDeveloperExceptionPage();
		else
		{
			app.UseHttpsRedirection();
			app.UseHsts();
			app.UseMiddleware<QueryLoggerMiddleware>();
		}

		app.UseMiddleware<ErrorLoggerMiddleware>();

		app.UseCors();
		app.UseResponseCaching();
		app.UseAuthorization();
		app.UseRateLimiter();

		app.MapControllers();

		app.Run();
	}

	private static void ConfigureServices()
	{
		_builder?.Services.AddRateLimiter(rateLimiterOptions =>
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

		_builder?.Services.AddCors(options =>
		{
			options.AddDefaultPolicy(policy =>
			{
				policy.WithOrigins("https://thejaviertc.github.io").AllowAnyMethod().AllowAnyHeader();
			});
		});

		_builder?.Services.AddResponseCaching();
		_builder?.Services.AddControllers();
	}

	private static void ConfigureHttpClients()
	{
		_builder?.Services.AddHttpClient(
			"SteamClient",
			client =>
			{
				client.BaseAddress = new Uri("https://api.steampowered.com/");
			}
		);

		_builder?.Services.AddHttpClient(
			"DiscordClient",
			client =>
			{
				client.BaseAddress = new Uri("https://discord.com/api/webhooks/");
			}
		);
	}
}
