using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace restApi.Extensiones
{
    public class ErrorHandler : IErrorHandler
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase ErrorHandler.
        /// </summary>
        public ErrorHandler()
        {
        }

        #endregion

        #region Métodos Públicos

        #region Miembros de IErrorHandler

        /// <summary>
        /// Método que realiza la captura del error en el servicio WCF.
        /// </summary>
        /// <param name="error">El error capturado en el servicio WCF.</param>
        /// <returns>True en caso de que sea una FaultException, false en caso contrario.</returns>
        bool IErrorHandler.HandleError(Exception error)
        {
            if (error is FaultException)
            {
                // Let WCF do normal processing
                return false;
            }
            else
            {
                // Fault message is already generated
                return true;
            }
        }

        /// <summary>
        /// Método que provee del error al canal abierto del cliente con el servicio WCF.
        /// </summary>
        /// <param name="error">El error que se generó en el servicio WCF.</param>
        /// <param name="version">La versión del servicio WCF.</param>
        /// <param name="fault">El FaultExceptio que se va a enviar al cliente.</param>
        void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            //LogManager.GetLogger("Logger").Fatal("Error en Servicio", error);

            if (error is FaultException)
            {
                // Let WCF do normal processing
            }
            else
            {
                // Generate fault message manually
                MessageFault messageFault = MessageFault.CreateFault(new FaultCode("Sender"), new FaultReason(error.Message), error, new NetDataContractSerializer());
                fault = Message.CreateMessage(version, messageFault, null);
            }
        }

        #endregion

        #endregion
    }
}
