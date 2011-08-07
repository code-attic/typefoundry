using Machine.Specifications;

namespace typefoundry.tests
{
    public abstract class with_assembly_scanning_for_marker_interface
    {
        private Establish context = () => 
            Foundry
                .Dependencies(x => x.Scan(s => s.AddAllTypesOf<AnInterfaceOf>() ));

    }
}