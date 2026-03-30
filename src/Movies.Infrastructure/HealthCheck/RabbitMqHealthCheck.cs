using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;

namespace Movies.Infrastructure.HealthCheck
{
    public class RabbitMqHealthCheck : IHealthCheck
    {
        private readonly ConnectionFactory _factory;

        public RabbitMqHealthCheck(ConnectionFactory factory)
        {
            _factory = factory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                using var connection = await _factory.CreateConnectionAsync(cancellationToken);

                await using (connection)
                {
                    return HealthCheckResult.Healthy("RabbitMQ OK");
                }
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy($"RabbitMQ unreachable: {ex.Message}", ex);
            }
        }
    }
}
