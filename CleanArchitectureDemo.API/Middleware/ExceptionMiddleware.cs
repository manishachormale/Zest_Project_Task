namespace CleanArchitectureDemo.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate requestDelegate, ILogger<ExceptionMiddleware> logger)
        {
            _next = requestDelegate;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";
                var response = new
                {
                    StatusCode = 500,
                    Message = "Internal server error",
                    Details = ex.Message
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
