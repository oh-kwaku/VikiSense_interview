using Microsoft.AspNetCore.ResponseCompression;
using VikiSense_interview.Extension_Methods;
using VikiSense_interview.Middlewares;
using VikiSense_interview.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddResponseCompression(options =>
{
    //options.EnableForHttps = true;
    //options.Providers.Add<BrotliCompressionProvider>();
    //options.Providers.Add<GzipCompressionProvider>();
    options.Providers.Add<CustomCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] { "application/json", "application/xml" });
    
});
var app = builder.Build();

// Configure the HTTP request pipeline. 

app.UseResponseCompression();
app.UseHttpsRedirection();


//app.UseCustomErrorHandlingMiddleware();
app.UseRequestLoggingMiddleware();
app.UseCustomErrorHandlingMiddleware();
app.MapControllers();

 
 

app.Run();
