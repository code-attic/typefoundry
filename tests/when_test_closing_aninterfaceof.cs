using Machine.Specifications;
using typefoundry.Impl.Scan;

namespace typefoundry.tests
{
    public class when_test_closing_aninterfaceof
    {
        static bool closes;
        static bool open;

        private Because of = () => 
                                 { 
                                     open = typeof( AnInterfaceOf<> ).IsOpenGeneric();
                                     closes = typeof( ClosedClass ).Closes( typeof( AnInterfaceOf<> ) );
                                 };

        private It should_close = () => closes.ShouldBeTrue();
        private It should_open_close = () => open.ShouldBeTrue();
    }
}