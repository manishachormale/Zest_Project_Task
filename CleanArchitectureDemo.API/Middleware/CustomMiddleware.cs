namespace CleanArchitectureDemo.API.Middleware
{
    public class CustomMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CustomMiddleware> _logger;

        public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            Console.WriteLine("Request Started");
            Console.WriteLine($"Method:{context.Request.Method}");
            Console.WriteLine($"URL:{context.Request.Path}");
            await _next(context);
            Console.WriteLine("Request Finished");
        }
    }
}
