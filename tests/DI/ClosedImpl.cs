using System;

namespace typefoundry.tests.DI
{
    public class ClosedImpl : ITakeGenericParams<string>
    {
        public Type GetTypeOfT
        {
            get { return typeof( string ); }
        }
    }
}