namespace typefoundry.tests.DI
{
    public class MessageHazzer : IHazzaMessage
    {
        public string GetMessage()
        {
            return "This is a message from MessageHazzer. Hi!";
        }
    }
}
