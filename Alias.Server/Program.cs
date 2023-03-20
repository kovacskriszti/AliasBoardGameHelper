using Alias.Server.Hubs;
using Alias.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IGameService, GameService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(options =>
    {
        options.WithOrigins("http://localhost:4200", "http://192.168.1.139:4200", "http://192.168.1.*:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
        .AllowCredentials();
    });
});
builder.Services.AddSignalR();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors();
}

app.UseAuthorization();
if (app.Environment.IsProduction())
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.MapHub<GameHub>("/game");
app.Run();
