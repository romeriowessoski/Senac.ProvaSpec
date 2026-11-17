using Microsoft.EntityFrameworkCore;
using Senac.ProvaSpec.Api;
using Senac.ProvaSpec.Application;
using Senac.ProvaSpec.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("LapTime")
    ?? $"Data Source={Path.Combine(builder.Environment.ContentRootPath, "lap-time.db")}";

builder.Services.AddDbContext<LapTimeDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddScoped<ILapRepository, SqliteLapRepository>();
builder.Services.AddScoped<LapTimeApplicationService>();
builder.Services.AddRazorPages();

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LapTimeDbContext>();
    await dbContext.Database.EnsureCreatedAsync();
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.MapLapEndpoints();

app.Run();
