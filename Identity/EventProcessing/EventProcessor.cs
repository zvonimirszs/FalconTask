using System.Text.Json;
using AutoMapper;
using Identity.Data;
using Identity.Dtos;
using Identity.Models;


namespace Identity.EventProcessing;
/// <summary>
/// Server klasa - konzumacija događaja
/// </summary>
public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;

    public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper)
    {
        Console.WriteLine("--> Event contructor");

        _scopeFactory = scopeFactory;
        _mapper = mapper;
    }
    /// <summary>
    /// Konzumacija događaja
    /// </summary>
    /// <param name="message">primljena poruka</param>
    public void ProcessEvent(string message)
    {
        Console.WriteLine("--> Process Event");
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.FilmPublished:
                profileRequest(message);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// Koji je događaj
    /// </summary>
    /// <param name="notifcationMessage">primljena poruka</param>
    private EventType DetermineEvent(string notifcationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

        switch(eventType.Event)
        {
            case "Film_Published":
                Console.WriteLine("--> Film Published Event Detected");
                return EventType.FilmPublished;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }
    // private void addRequest(string entityPublishedMessage)
    // {

    // }
    /// <summary>
    /// Prfilizacija requesta
    /// </summary>
    /// <param name="requestPublishedMessage">primljena poruka</param>
    private void profileRequest(string requestPublishedMessage)
    {
        Console.WriteLine($"--> Profile Request:{requestPublishedMessage}");
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<IIdentityRepo>();
            
            var requestPublishedDto = JsonSerializer.Deserialize<FilmPublishedDto>(requestPublishedMessage);
        }
    }
}

enum EventType
{
    FilmPublished,
    Undetermined
}
