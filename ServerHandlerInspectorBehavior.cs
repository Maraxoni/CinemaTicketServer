using CoreWCF.Channels;
using CoreWCF.Description;
using CoreWCF.Dispatcher;

namespace CinemaTicketServer
{
    public class ServerHandlerInspectorBehavior : IEndpointBehavior
    {
        private readonly IDispatchMessageInspector _inspector;

        public ServerHandlerInspectorBehavior(IDispatchMessageInspector inspector)
        {
            Console.WriteLine($"SB0");
            _inspector = inspector;

        }

        void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
            Console.WriteLine($"SB1");
        }

        void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            Console.WriteLine($"SB2");
        }

        void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
            Console.WriteLine($"SB3");
        }

        void IEndpointBehavior.Validate(ServiceEndpoint endpoint)
        {
            Console.WriteLine($"SB4");
        }
    }
}
