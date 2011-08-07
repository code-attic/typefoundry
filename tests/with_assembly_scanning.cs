using Machine.Specifications;

namespace typefoundry.tests
{
    public interface AnInterfaceOf<T> : AnInterfaceOf
    {
        
    }

    public abstract class with_assembly_scanning
    {
        private Establish context = () =>
                                        {
                                            Foundry
                                                .Dependencies(x => x.Scan(s =>
                                                                              {
                                                                                  s.AddAllTypesOf<IAmAnInterface>();
                                                                              }));
                                        };

    }
}
