using Machine.Specifications;
using typefoundry.Impl;

namespace typefoundry.tests.DI
{
    public class with_singleton_registration : with_container
    {
        private Establish context = () =>
            {
                SingletonA.Reset();
                SingletonB.Reset();

                var def1 = DependencyExpression.For<IShouldBeSingleton>("a");
                def1.Use<SingletonA>().AsSingleton();

                var def2 = DependencyExpression.For<IShouldBeSingleton>("b");
                def2.Use( new SingletonB() );

                Container.Register( def1 );
                Container.Register( def2 );
            };
    }
}