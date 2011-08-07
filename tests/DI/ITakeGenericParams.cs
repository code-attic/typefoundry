using System;

namespace typefoundry.tests.DI
{
    public interface ITakeGenericParams<T>
    {
        Type GetTypeOfT { get; }
    }
}