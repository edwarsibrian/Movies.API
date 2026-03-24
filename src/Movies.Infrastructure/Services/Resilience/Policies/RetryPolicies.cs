using Polly;
using Polly.Retry;

namespace Movies.Infrastructure.Services.Resilience.Policies
{
    public class RetryPolicies
    {
        public static AsyncRetryPolicy DefaultRetry =>
            Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                2,
                attempt => TimeSpan.FromSeconds(2),
                (exception, timeSpan, retryCount, context) =>
                {
                    //logging
                    Console.WriteLine($"Retry {retryCount}: {exception.Message}");
                });
    }
}
