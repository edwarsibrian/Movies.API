using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Movies.Domain.Common.Interfaces;
using Movies.Infrastructure.Helpers;
using Movies.Infrastructure.Services.FileStorages;
using Movies.Infrastructure.Settings;

namespace Movies.Infrastructure.Services
{
    public class SyncHostedService : BackgroundService
    {
        private readonly ILogger<SyncHostedService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly SyncSettings _syncSettings;
        private readonly FileStorageSettings _fileSettings;
        private readonly HealthCheckService _healthCheckService;
        private readonly string _basePath;

        public SyncHostedService(
            ILogger<SyncHostedService> logger,
            IServiceProvider serviceProvider,
            IOptions<SyncSettings> syncOptions,
            IOptions<FileStorageSettings> fileOptions,
            HealthCheckService healthCheckService,
            IHostEnvironment env)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _syncSettings = syncOptions.Value;
            _fileSettings = fileOptions.Value;
            _healthCheckService = healthCheckService;

            _basePath = Path.IsPathRooted(_fileSettings.LocalStoragePath)
                ? _fileSettings.LocalStoragePath
                : Path.Combine(env.ContentRootPath, _fileSettings.LocalStoragePath);
            Directory.CreateDirectory(_basePath);
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
                    var files = EnumerateFiles(containerKey).ToList();
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
                    string pictureUrl = string.Empty;
                    using var scope = _serviceProvider.CreateScope();
                    var storageService = scope.ServiceProvider.GetRequiredService<AzureFileStorageService>();
                    pictureUrl = await storageService.UploadFromFilePathAsync(localPath, fileName, containerKey);

                    //Update Actor
                    var actorRepository = scope.ServiceProvider.GetRequiredService<IActorRepository>();
                    var actor = await actorRepository.GetActorByFileNameAsync(fileName, cancellationToken);
                    if (actor != null)
                    {
                        actor.Picture = pictureUrl;
                        await actorRepository.UpdateAsync(actor, cancellationToken);
                    }
                    else
                    {
                        _logger.LogWarning("Actor not found for file {file}, pictureUrl {picture}", fileName, pictureUrl);
                    }

                    _logger.LogInformation("Uploaded file {file} to Azure container {container} on attempt {attempt}", fileName, containerKey, attempt);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Attempt {attempt} failed uploading {file} to Azure, will retry", attempt, fileName);
                    await Task.Delay(TimeSpan.FromSeconds(_syncSettings.RetryDelaySeconds), cancellationToken);
                }
            }

            _logger.LogError("Failed to upload {file} to Azure after {attempts} attempts", fileName, attempt);
            return false;
        }

        /// <summary>
        /// Helper for list files pending in a directory container
        /// </summary>
        /// <param name="container"></param>
        /// <returns></returns>
        private IEnumerable<string> EnumerateFiles(string container)
        {
            var directory = DirectoryHelper.GetDirectory(_basePath, _fileSettings.Containers, container);
            return Directory.EnumerateFiles(directory, "*", SearchOption.TopDirectoryOnly);
        }
    }
}
