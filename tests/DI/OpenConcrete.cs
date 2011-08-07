namespace typefoundry.tests.DI
{
    public class OpenConcrete<T>
    {
        public IMessageProvider Provider { get; set; }

        public OpenConcrete( IMessageProvider provider )
        {
            Provider = provider;
        }
    }
}