using System;
using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace restApi.Extensiones
{
    public class ServiceBehavior : Attribute, IServiceBehavior
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase ServiceBehavior.
        /// </summary>
        public ServiceBehavior()
        {
        }
        #endregion

        #region Métodos Públicos

        #region Miembros de IServiceBehavior

        /// <summary>
        /// Método que se utiliza al agregar los parámetros de los enlaces del servicio WCF.
        /// </summary>
        /// <param name="serviceDescription">Parámetro (serviceDescription) de evento.</param>
        /// <param name="serviceHostBase">Parámetro (serviceHostBase) de evento.</param>
        /// <param name="endpoints">Parámetro (endpoints) de evento.</param>
        /// <param name="bindingParameters">Parámetro (bindingParameters) de evento.</param>
        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
            return;
        }

        /// <summary>
        /// Método que se utiliza al agregar el comportamiento del servicio WCF.
        /// </summary>
        /// <param name="serviceDescription">Parámetro (serviceDescription) de evento.</param>
        /// <param name="serviceHostBase">Parámetro (serviceHostBase) de evento.</param>
        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                this.ApplyDispatchBehavior(dispatcher);
            }
        }

        /// <summary>
        /// Método que se utiliza al validar el servicio WCF.
        /// </summary>
        /// <param name="serviceDescription">Parámetro (serviceDescription) de evento.</param>
        /// <param name="serviceHostBase">Parámetro (serviceHostBase) de evento.</param>
        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            return;
        }

        #endregion

        #endregion

        #region Métodos Privados

        /// <summary>
        /// Método que se utiliza al aplicar el comportamiento del servicio WCF.
        /// </summary>
        /// <param name="dispatcher">Parámetro (dispatcher) de evento.</param>
        private void ApplyDispatchBehavior(ChannelDispatcher dispatcher)
        {
            foreach (IErrorHandler errorHandler in dispatcher.ErrorHandlers)
            {
                if (errorHandler is ErrorHandler)
                {
                    return;
                }
            }

            dispatcher.ErrorHandlers.Add(new ErrorHandler());
        }

        #endregion
    }
}
