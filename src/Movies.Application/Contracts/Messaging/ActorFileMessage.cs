namespace Movies.Application.Contracts.Messaging
{
    public class ActorFileMessage : FileMessage
    {
        public int ActorId { get; set; }
    }
}
