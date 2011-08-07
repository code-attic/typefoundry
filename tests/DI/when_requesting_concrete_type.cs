using Machine.Specifications;

namespace typefoundry.tests.DI
{
    public class when_requesting_concrete_type : with_simple_registration
    {
        private static string message;
        private Because of = () =>
            {
                var instance = Container.GetInstance<PlainConcrete>();
                message = instance.Provider.GetMessage();
            };

        private It should_have_correct_message = () => message.ShouldEqual( "This is a message from MessageHazzer. Hi!" );
    }
}
