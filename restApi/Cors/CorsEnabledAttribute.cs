using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.Text;
using System.Threading.Tasks;

namespace restApi
{
    public class CorsEnabledAttribute : Attribute, IOperationBehavior, IServiceBehavior
    {
        public void AddBindingParameters(OperationDescription operationDescription, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(OperationDescription operationDescription, ClientOperation clientOperation)
        {
        }

        public void ApplyDispatchBehavior(OperationDescription operationDescription, DispatchOperation dispatchOperation)
        {
        }

        public void Validate(OperationDescription operationDescription)
        {
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        { } //implementation not needed

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints,
                                         BindingParameterCollection bindingParameters)
        { } //implementation not needed

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcherBase chanDispBase in serviceHostBase.ChannelDispatchers)
            {
                ChannelDispatcher channelDispatcher = chanDispBase as ChannelDispatcher;
                if (channelDispatcher == null)
                    continue;
                channelDispatcher.ErrorHandlers.Add(new SvcErrorHandler());
            }
        }
    }

    public class SvcErrorHandler : IErrorHandler
    {
        public bool HandleError(Exception error)
        {
            //You can log th message if you want.
            return true;
        }

        public void ProvideFault(Exception error, MessageVersion version, ref Message msg)
        {
            if (error is FaultException)
                return;

            FaultException faultException = new FaultException(error.Message);
            MessageFault messageFault = faultException.CreateMessageFault();
            msg = Message.CreateMessage(version, messageFault, faultException.Action);
        }
    }
}
