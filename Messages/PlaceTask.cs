using NServiceBus;

namespace Messages
{
    public class PlaceTask:
        ICommand
    {
        public int TaskId { get; set; }
    }
}
