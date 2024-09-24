using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Middelwares
{
    public class ExceptionMiddelware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<ExceptionMiddelware> logger;
        private readonly IHostEnvironment env;

        public ExceptionMiddelware(RequestDelegate next , ILogger<ExceptionMiddelware> logger , IHostEnvironment env) 
        {
            this.next=next;
            this.logger=logger;
            this.env=env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next.Invoke(context); // Move to the next middelware
            }
            catch (Exception e) 
            {
                logger.LogError(e , e.Message);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                var exceptionErrorResponse = env.IsDevelopment() ?
                    new ApisExceptionResponse(500 , e.Message , e.StackTrace.ToString()) 
                    :
                    new ApisExceptionResponse(500);

                var options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(exceptionErrorResponse);

                await context.Response.WriteAsync(json);

            }
        }
    }
}
