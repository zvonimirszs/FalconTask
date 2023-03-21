namespace Identity.EventProcessing;
/// <summary>
/// Server sučelje - konzumacija događaja
/// </summary>
public interface IEventProcessor
{
    void ProcessEvent(string message);
}
