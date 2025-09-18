namespace Movies.API.Middlewares
{
    public sealed class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (FluentValidation.ValidationException ex)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                var errors = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage });
                var response = new
                {
                    Message = "Validation failed",
                    Errors = errors
                };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
