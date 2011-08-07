namespace typefoundry.tests.DI
{
    public class PlainConcrete
    {
        public IMessageProvider Provider { get; set; }

        public PlainConcrete( IMessageProvider provider )
        {
            Provider = provider;
        }
    }
}