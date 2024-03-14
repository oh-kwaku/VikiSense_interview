using VikiSense_interview.Extension_Methods;
using VikiSense_interview.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline. 

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCustomErrorHandlingMiddleware();
app.UseRequestLoggingMiddleware();
app.UseCustomResponseCompressionMiddleware();

app.MapControllers();

 

app.Run();
