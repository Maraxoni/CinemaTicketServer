using CoreWCF.Description;
using CoreWCF.Dispatcher;
using CoreWCF.Channels;
using System.Collections.ObjectModel;
using System.ServiceModel;
using CoreWCF;
using System;

public class GlobalInspectorBehavior : IServiceBehavior
{
    private readonly IDispatchMessageInspector _inspector;

    public GlobalInspectorBehavior(IDispatchMessageInspector inspector)
    {
        // GI0: Constructor
        Console.WriteLine("GI0: GlobalInspectorBehavior initialized");
        _inspector = inspector;
    }

    public void AddBindingParameters(
        ServiceDescription serviceDescription,
        ServiceHostBase serviceHostBase,
        Collection<ServiceEndpoint> endpoints,
        BindingParameterCollection bindingParameters)
    {
        // GI1: AddBindingParameters
        Console.WriteLine("GI1: AddBindingParameters invoked");
    }

    public void ApplyDispatchBehavior(
        ServiceDescription serviceDescription,
        ServiceHostBase serviceHostBase)
    {
        // GI2: ApplyDispatchBehavior
        Console.WriteLine("GI2: ApplyDispatchBehavior invoked");
        foreach (ChannelDispatcher cd in serviceHostBase.ChannelDispatchers)
        {
            foreach (var ed in cd.Endpoints)
            {
                ed.DispatchRuntime.MessageInspectors.Add(_inspector);
            }
        }
    }

    public void Validate(
        ServiceDescription serviceDescription,
        ServiceHostBase serviceHostBase)
    {
        // GI3: Validate
        Console.WriteLine("GI3: Validate invoked");
    }
}
