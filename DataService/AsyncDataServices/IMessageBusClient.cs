using DataService.Dtos;

namespace DataService.AsyncDataServices
{
    /// <summary>
    /// Sučelje IMessageBusClient
    /// </summary>
    public interface IMessageBusClient
    {
        
        void PublishNewFilm(FilmPublishedDto filmPublishedDto);
    }
}