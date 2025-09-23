namespace Movies.API.Middlewares
{
    public class PaginationHeaderMiddleware
    {
        private readonly RequestDelegate _next;
        public PaginationHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            context.Response.OnStarting(() =>
            {
                // After the response is generated, check for pagination info in the items
                if (context.Items.TryGetValue("TotalRecords", out var totalRecordsObj) && totalRecordsObj is int totalRecords)
                {
                    // Add the TotalRecords header to the response
                    context.Response.Headers.Append("TotalRecords", totalRecords.ToString());
                }
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
