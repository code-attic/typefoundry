using Machine.Specifications;
using typefoundry.Impl;

namespace typefoundry.tests.DI
{
    public class with_container
    {
        protected static SimpleDependencyRegistry Container { get; set; }
        private Establish context = () => { Container = new SimpleDependencyRegistry(); };
    }
}