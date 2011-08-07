using System;

namespace typefoundry.tests.DI
{
    public class OpenImpl<T> : ITakeGenericParams<T>
    {
        public Type GetTypeOfT
        {
            get { return typeof( T ); }
        }
    }
}