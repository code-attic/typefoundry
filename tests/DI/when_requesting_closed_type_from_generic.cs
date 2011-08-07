using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;

namespace typefoundry.tests.DI
{
    public class when_requesting_closed_type_from_generic : with_closed_types_registered
    {
        protected static AClassOf<string> instance;
        protected static List<AClassOf<string>> instances;
        private Because of = () =>
            {
                instance = Container.GetInstance<AClassOf<string>>();
                instances = Container.GetAllInstances<AClassOf<string>>().ToList();
            };

        private It should_have_one_instance_in_collection = () => instances.Count.ShouldEqual( 1 );
        private It should_have_correct_instance_type = () => instance.ShouldBe( typeof( ClosedClass ) );
        private It should_have_same_type_in_list_as_instance = () => instances.First().GetType().ShouldEqual( instance.GetType() );
    }
}
