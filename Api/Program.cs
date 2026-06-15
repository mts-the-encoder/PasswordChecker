using Application;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Register Swagger/OpenAPI services using Swashbuckle
// Learn more: https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CleanArch.WebApi v1"));
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
// Expose a Program class for integration tests (WebApplicationFactory)
public partial class Program { }
