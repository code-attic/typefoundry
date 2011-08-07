using Machine.Specifications;

namespace typefoundry.tests.DI
{
    public class when_testing_duplicate_singleton_definition : with_duplicate_singletons_registered
    {
        private static bool pass;
        private Because of = () =>
            {
                var instance = Container.GetInstance<IShouldBeSingleton>();
                pass = ( instance as SingletonB ).Message == "second";
            };

        private It should_overwrite_initial_dependency_with_latter = () => pass.ShouldBeTrue();
    }
}