using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movies.Infrastructure.Services.FileStorages;
using Movies.Infrastructure.Settings;

namespace Movies.Infrastructure.Services
{
    public class SyncHostedService : BackgroundService
    {
        private readonly ILogger<SyncHostedService> _logger;
        private readonly AzureFileStorageService _azure;
        private readonly LocalFileStorageService _local;
        private readonly SyncSettings _syncSettings;
        private readonly FileStorageSettings _fileSettings;
        private readonly HealthCheckService _healthCheckService;

        public SyncHostedService(
            ILogger<SyncHostedService> logger,
            AzureFileStorageService azure,
            LocalFileStorageService local,
            IOptions<SyncSettings> syncOptions,
            IOptions<FileStorageSettings> fileOptions,
            HealthCheckService healthCheckService)
        {
            _logger = logger;
            _azure = azure;
            _local = local;
            _syncSettings = syncOptions.Value;
            _fileSettings = fileOptions.Value;
            _healthCheckService = healthCheckService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("SyncHostedService started. Interval: {interval}s", _syncSettings.IntervalSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Revisa healthchecks - filtramos por nombre o tag
                    var report = await _healthCheckService.CheckHealthAsync(st => st.Tags.Contains("azure") || st.Name == "Azure Blob Storage", stoppingToken);

                    if (report.Status == HealthStatus.Healthy)
                    {
                        _logger.LogInformation("Azure healthy => starting sync.");
                        await SyncAllContainersAsync(stoppingToken);
                    }
                    else
                    {
                        _logger.LogInformation("Azure not healthy ({status}) - skipping sync.", report.Status);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in SyncHostedService main loop");
                }

                await Task.Delay(TimeSpan.FromSeconds(_syncSettings.IntervalSeconds), stoppingToken);
            }
        }

        private async Task SyncAllContainersAsync(CancellationToken cancellationToken)
        {
            foreach (var containerKey in _fileSettings.Containers.Keys)
            {
                try
                {
                    var files = _local.EnumerateFiles(containerKey).ToList();
                    _logger.LogInformation("Sync: found {count} files in container {container}", files.Count, containerKey);

                    foreach (var path in files)
                    {
                        if (cancellationToken.IsCancellationRequested) break;

                        var fileName = Path.GetFileName(path);
                        var success = await TryUploadWithRetriesAsync(path, fileName, containerKey, _syncSettings.MaxRetries, cancellationToken);
                        if (success)
                        {
                            try
                            {
                                File.Delete(path);
                                _logger.LogInformation("Deleted local file after successful upload: {path}", path);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Could not delete local file {path} after upload", path);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error syncing container {container}", containerKey);
                }
            }
        }

        private async Task<bool> TryUploadWithRetriesAsync(string localPath, string fileName, string containerKey, int maxRetries, CancellationToken cancellationToken)
        {
            var attempt = 0;
            while (attempt < maxRetries && !cancellationToken.IsCancellationRequested)
            {
                attempt++;
                try
                {
                    await _azure.UploadFromFilePathAsync(localPath, fileName, containerKey);
                    _logger.LogInformation("Uploaded file {file} to Azure container {container} on attempt {attempt}", fileName, containerKey, attempt);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Attempt {attempt} failed uploading {file} to Azure, will retry", attempt);
                    await Task.Delay(TimeSpan.FromSeconds(_syncSettings.RetryDelaySeconds), cancellationToken);
                }
            }

            _logger.LogError("Failed to upload {file} to Azure after {attempts} attempts", fileName, attempt);
            return false;
        }
    }
}
