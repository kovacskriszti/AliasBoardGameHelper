using Alias.Server.Hubs;
using Alias.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddTransient<IGameService, GameService>();
builder.Services.AddCors(options =>
{
	options.AddDefaultPolicy(options =>
	{
		options.WithOrigins("http://localhost:4200", "http://192.168.1.136:4200", "http://192.168.1.*:4200")
						.AllowAnyHeader()
						.AllowAnyMethod()
		.AllowCredentials();
	});
});
builder.Services.AddControllers();
builder.Services.AddSignalR();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
	app.UseCors();
}

app.UseAuthorization();
if (app.Environment.IsProduction())
{
	app.UseDefaultFiles();
	app.UseStaticFiles();
}

app.MapControllers();
app.MapHub<GameHub>("/game");
app.Run();
