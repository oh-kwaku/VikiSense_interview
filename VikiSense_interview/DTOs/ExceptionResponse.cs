using System.Net;

namespace VikiSense_interview.DTOs;

public record ExceptionResponse(HttpStatusCode StatusCode, string Description);

