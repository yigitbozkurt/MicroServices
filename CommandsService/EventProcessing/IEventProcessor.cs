namespace CommandsService.EventProcessing
{
    public interface IEventProcessor
    {
        public void ProcessEvent (string message);
    }
}