using System.Text.Json;
using AutoMapper;
using CommandsService.Data;
using CommandsService.Dtos;
using CommandsService.Models;
using Microsoft.Extensions.DependencyInjection;

namespace CommandsService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory , AutoMapper.IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch(eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;        
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            System.Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch(eventType.Event)
            {
                case "Platform Published" : 
                    System.Console.WriteLine("--> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                System.Console.WriteLine("Could not determine the event type");
                return EventType.Undetermined;
            }
        
        }

        private void AddPlatform(string platformPublishedMessage)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
            var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(platformPublishedMessage);

            try
            {
                var plat = _mapper.Map<Platform>(platformPublishedDto);
                if (!repo.ExternalPlatformExists(plat.ExternalId))
                {
                    repo.CreatePlatform(plat);
                    repo.SaveChanges();
                    System.Console.WriteLine("--> Platform added.");
                }
                else
                {
                    System.Console.WriteLine("--> Platform already exist.");
                }
            }
            catch(Exception ex)
            {
                System.Console.WriteLine($"Could not add Platform to DB: {ex.Message}");
            }
            }
        }

        enum EventType
        {
            PlatformPublished,
            Undetermined
        }
    }
}