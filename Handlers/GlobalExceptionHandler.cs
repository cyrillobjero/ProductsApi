using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ProductsApi.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace ProductsApi.Handlers
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) => _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
            HttpContext context, Exception ex, CancellationToken ct)
        {
            var (status, title) = ex switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Resource not found"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
                ValidationException => (StatusCodes.Status400BadRequest, "Validation failed"),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
            };

            _logger.LogError(ex, "Unhandled exception ({Status})", status);

            var problem = new ProblemDetails
            {
                Status = status,
                Title = title,
                Detail = status == 500 ? null : ex.Message
            };

            context.Response.StatusCode = status;
            await context.Response.WriteAsJsonAsync(problem, ct);
            return true;
        }
    }
}

