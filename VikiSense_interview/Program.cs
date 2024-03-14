using VikiSense_interview.Extension_Methods;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline. 

app.UseHttpsRedirection();
//app.UseResponseCompression();
app.UseCustomResponseCompressionMiddleware();
app.UseRequestLoggingMiddleware();
app.UseCustomErrorHandlingMiddleware();
app.MapControllers();

app.Run();
