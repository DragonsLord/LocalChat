using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace ConsoleServer
{
    public class BehaviorWithExceptions: IServiceBehavior
    {
        public void AddBindingParameters(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase, System.Collections.ObjectModel.Collection<ServiceEndpoint> endpoints, System.ServiceModel.Channels.BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
        }

        public void Validate(ServiceDescription serviceDescription, System.ServiceModel.ServiceHostBase serviceHostBase)
        {
            var error_handler = new ChatErrorHandler();

            foreach (var chanel in serviceHostBase.ChannelDispatchers)
            {
                if (chanel is ChannelDispatcher)
                    ((ChannelDispatcher)chanel).ErrorHandlers.Add(error_handler);
            }
        }
    }
}
