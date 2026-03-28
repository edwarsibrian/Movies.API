using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Movies.Application.Contracts.Messaging;
using Movies.Application.Interfaces;
using Movies.Domain.Common.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text.Json;

namespace Movies.Infrastructure.Messaging.Consumers
{
    public class ActorImageConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ConnectionFactory _factory;
        private readonly ICacheService _cacheService;
        
        private const string CacheKey = "ActorsCache";

        public ActorImageConsumer(
            IServiceScopeFactory scopeFactory,  
            ConnectionFactory factory, 
            ICacheService cacheService)
        {
            _scopeFactory = scopeFactory;
            _factory = factory;
            _cacheService = cacheService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = await _factory.CreateConnectionAsync();
            var channel = await connection.CreateChannelAsync();

            await channel.QueueDeclareAsync(
                queue: QueueNames.ActorImageUploadQueue,
                durable: false,
                exclusive: false,
                autoDelete: false
                );

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.ReceivedAsync += async (model, ea) =>
            {
                using var scope = _scopeFactory.CreateScope();

                var _fileStorageService = scope.ServiceProvider.GetRequiredService<IFileStorageService>();
                var _actorRepository = scope.ServiceProvider.GetRequiredService<IActorRepository>();

                var message = JsonSerializer.Deserialize<ActorFileMessage>(ea.Body.ToArray());

                if (message is null)
                {
                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                    return;
                }

                bool success = false;

                try
                {
                    switch (message.Action)
                    {
                        case FileAction.Create:
                            using (var stream = File.OpenRead(message.FilePath))
                            {
                                var url = await _fileStorageService.SaveFileAsync(
                                    stream, message.FileName, message.ContainerName);

                                await _actorRepository.UpdatePictureAsync(message.ActorId, url, stoppingToken);
                                await _cacheService.EvictByTagAsync(CacheKey, CancellationToken.None);
                            }
                            break;
                        case FileAction.Edit:
                            using (var stream = File.OpenRead(message.FilePath))
                            {
                                var url = await _fileStorageService.EditFileAsync(
                                    message.ExistingFilePath,
                                    stream,
                                    message.FileName,
                                    message.ContainerName);

                                await _actorRepository.UpdatePictureAsync(message.ActorId, url, stoppingToken);
                                await _cacheService.EvictByTagAsync(CacheKey, CancellationToken.None);
                            }
                            break;
                        case FileAction.Delete:
                            // delete
                            break;
                    }

                    success = true;

                    await channel.BasicAckAsync(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    // Log the exception (not implemented here)
                    Console.WriteLine($"Error processing message: {ex.Message}");

                    await channel.BasicNackAsync(ea.DeliveryTag, false, true);
                }
                finally
                {
                    if (success &&
                    !string.IsNullOrEmpty(message.FilePath) &&
                    File.Exists(message.FilePath))
                    {
                        File.Delete(message.FilePath);
                    }
                }
            };

            await channel.BasicConsumeAsync(
                queue: QueueNames.ActorImageUploadQueue,
                autoAck: false,
                consumer: consumer
                );

            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
    }
}
