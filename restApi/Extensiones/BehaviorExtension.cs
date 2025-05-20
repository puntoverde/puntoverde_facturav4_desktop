using System;
using System.ServiceModel.Configuration;
namespace restApi.Extensiones
{
    public class BehaviorExtension : BehaviorExtensionElement
    {
        #region Constructores

        /// <summary>
        /// Inicializa una nueva instancia de la clase BehaviorExtension.
        /// </summary>
        public BehaviorExtension()
        {
        }

        #endregion

        #region Propiedades

        /// <summary>
        /// Propiedad que representa el tipo de comportamiento del servicio.
        /// </summary>
        public override Type BehaviorType
        {
            get
            {
                return typeof(ServiceBehavior);
            }
        }

        #endregion

        #region Métodos Protegidos

        /// <summary>
        /// Método que genera el comportamiento del servicio WCF.
        /// </summary>
        /// <returns>Una nueva instancia del comportamiento del servicio WCF.</returns>
        protected override object CreateBehavior()
        {
            return new ServiceBehavior();
        }

        #endregion
    }
}
