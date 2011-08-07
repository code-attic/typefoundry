namespace typefoundry.tests.DI
{
    public class MessageProvider
        : IMessageProvider
    {
        public IHazzaMessage MessageHaz { get; set; }

        public string GetMessage()
        {
            return MessageHaz.GetMessage();
        }

        public MessageProvider( IHazzaMessage messageHaz )
        {
            MessageHaz = messageHaz;
        }
    }
}