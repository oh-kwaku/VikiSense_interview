## Requirement
- .Net 7 is required.


## Instruction

I used Visual Studio project template which already has ``WeatherForecastController`` and some endpoints to start with.
- The base URL is   ```http://localhost:5193```.
- See the application console for logs and exception details (this prevents showing sensitive information and unfriendly errors to the client)
- To see query params logging, go to ```/WeatherForecast/summaries?itemsCount=6```
- To see the global exception middleware response, supply an invalid index to the summaries endpoint example: ```/WeatherForecast/summaries/-1```

## Decision taken:
In the ```CustomResponseCompressionMiddleware``` class I retrieved the compression threshold from configuration by injecting IConfiguration.
I check the size of the response and if it's higher than the threshold then compression kicks in, otherwise the response is not compressed.

In the ```GlobalExceptionHandlingMiddleware``` class,  I check the exception type to determine the proper error message to show. A generic error message is shown for unhandled/unknown exception types.

To make the ```Program.cs``` file cleaner and to be consistent with how built-in middleware is used, I added extension methods for each middleware to follow ```app.Use*``` pattern. Example: ```app.UseCustomResponseCompressionMiddleware();
```, ```app.UseRequestLoggingMiddleware()```, ```app.UseCustomErrorHandlingMiddleware()```
