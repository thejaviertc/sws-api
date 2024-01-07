var builder = WebApplication.CreateBuilder(args);

// Services
builder.Services.AddControllers();

var app = builder.Build();

// Middlewares
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();