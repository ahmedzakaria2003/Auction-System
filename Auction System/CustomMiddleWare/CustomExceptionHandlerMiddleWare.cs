using AuctionSystem.Domain.Exceptions;
using AuctionSystem.Shared.ErrorHandling;
using System.Net;
using System.Text.Json;

namespace Auction_System.CustomMiddleWare
{
    public class CustomExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomExceptionHandlerMiddleware> _logger;

        public CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);

                await HandleNotFoundEndpointAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled Exception caught in middleware");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleNotFoundEndpointAsync(HttpContext context)
        {
            if (context.Response.StatusCode == (int)HttpStatusCode.NotFound)
            {
                var response = new ErrorToReturn
                {
                    StatusCode = (int)HttpStatusCode.NotFound,
                    ErrorMessage = $"Endpoint '{context.Request.Path}' not found."
                };

                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(response);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var response = new ErrorToReturn();
            context.Response.ContentType = "application/json";

            switch (ex)
            {
                case NotFoundException:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    response.StatusCode = context.Response.StatusCode;
                    response.ErrorMessage = ex.Message;
                    break;

                case UnauthorizedException:
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    response.StatusCode = context.Response.StatusCode;
                    response.ErrorMessage = ex.Message;
                    break;

                case BadRequestException badRequest:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = context.Response.StatusCode;
                    response.ErrorMessage = badRequest.Message;
                    response.Errors = badRequest.Errors;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusCode = context.Response.StatusCode;
                    response.ErrorMessage = "An unexpected error occurred.";
                    break;
            }

            await context.Response.WriteAsJsonAsync(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
        }
    }
}
