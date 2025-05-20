using entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using contpaqSDK;

namespace comercial
{
    public class DomicilioImplements
    {
        public static int createOrUpdateDomicilio(string CodigoCliente,EntityDomicilio domicilio) {
            int IError = 0;//variable para los errores de contpaq
            Console.WriteLine("Codigo clinte en buscar domicilio: {0}", CodigoCliente);

            IError = Direccion.fBuscaDireccionCteProv(CodigoCliente,0);
            SDK.MuestraError(IError, "fBuscaDireccionCteProv");
            if (IError == 0)
            {
                IError = updateDomicilio(CodigoCliente, domicilio); 
            }
            else {
                IError = createDomicilio(CodigoCliente, domicilio);
            }
            SDK.MuestraError(IError, "createOrUpdateDomicilio");
            return IError;
        }

        public static int createDomicilio(string CodigoCliente, EntityDomicilio domicilio)
        {
            int IError;
            int idDireccion = 0;

            Direccion.tDireccion direccion = new Direccion.tDireccion();
            direccion.cCodCteProv = CodigoCliente;
            direccion.cNombreCalle = domicilio.calle;
            direccion.cNumeroExterior = domicilio.numExt;
            direccion.cNumeroInterior = domicilio.numInt;
            direccion.cColonia = domicilio.colonia;
            direccion.cCodigoPostal = domicilio.cp;
            direccion.cCiudad = domicilio.municipio;
            direccion.cEstado = domicilio.estado;
            direccion.cPais = domicilio.pais;
            direccion.cTipoDireccion = 1;//0 es fiscal ,1 es envio  aunque es al reves ???

            //se registra ladireccion
            IError = Direccion.fAltaDireccion(ref idDireccion, ref direccion);
            SDK.MuestraError(IError, "fAltaDireccion");
            return IError;
        }

        public static int updateDomicilio(string CodigoCliente, EntityDomicilio domicilio)
        {
            int IError;

            Direccion.tDireccion direccion = new Direccion.tDireccion();
            direccion.cCodCteProv = CodigoCliente;
            direccion.cNombreCalle = domicilio.calle;
            direccion.cNumeroExterior = domicilio.numExt;
            direccion.cNumeroInterior = domicilio.numInt;
            direccion.cColonia = domicilio.colonia;
            direccion.cCodigoPostal = domicilio.cp;
            direccion.cCiudad = domicilio.municipio;
            direccion.cEstado = domicilio.estado;
            direccion.cPais = domicilio.pais;
            direccion.cTipoDireccion = 1;//0 es fiscal ,1 es envio  aunque es al reves ???

            //como la direccion existe se prosigue a actualizarla 
            IError = Direccion.fActualizaDireccion(ref direccion);
            SDK.MuestraError(IError, "fActualizaDireccion");
            return IError;
        }


    }
}
