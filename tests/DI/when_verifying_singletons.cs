using System.Linq;
using Machine.Specifications;
using typefoundry.Impl.Utility;

namespace typefoundry.tests.DI
{
    public class when_verifying_singletons : with_singleton_registration
    {
        private static IShouldBeSingleton a;
        private static IShouldBeSingleton b;

        private Because of = () =>
            {
                Enumerable
                    .Range( 0, 50 )
                    .AsParallel()
                    .ForEach( x => a = Container.GetInstance<IShouldBeSingleton>( "a" ) );
                
                Enumerable
                    .Range( 0, 50 )
                    .AsParallel()
                    .ForEach( x => b = Container.GetInstance<IShouldBeSingleton>( "b" ) );
            };

        private It should_only_have_single_a_instance = () => a.Instance.ShouldEqual( 0 );
        private It should_only_have_single_b_instance = () => b.Instance.ShouldEqual( 0 );
    }
}