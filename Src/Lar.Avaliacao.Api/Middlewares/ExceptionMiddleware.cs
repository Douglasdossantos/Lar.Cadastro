using Lar.Avaliacao.Application.Exceptions;
using Lar.Avaliacao.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace Lar.Avaliacao.Api.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await TratarExcecaoAsync(context, ex);
            }
        }
        private async Task TratarExcecaoAsync(HttpContext context, Exception exception)
        {
            var (statusCode, titulo) = exception switch
            {
                DomainException => (HttpStatusCode.BadRequest, "Requisição inválida"),
                NotFoundException => (HttpStatusCode.NotFound, "Recurso não encontrado"),
                ConflictException => (HttpStatusCode.Conflict, "Conflito de dados"),
                UnauthorizedException => (HttpStatusCode.Unauthorized, "Não autorizado"),
                _ => (HttpStatusCode.InternalServerError, "Erro interno no servidor")
            };

            if (statusCode == HttpStatusCode.InternalServerError)
                _logger.LogError(exception, "Erro não tratado durante o processamento da requisição.");

            var problemDetails = new
            {
                status = (int)statusCode,
                title = titulo,
                detail = exception.Message,
                traceId = context.TraceIdentifier
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
