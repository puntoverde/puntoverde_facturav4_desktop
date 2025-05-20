using contpaqSDK;
using entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comercial
{
    public class ClienteImplements
    {
        
        public static int createOrUpdateCliente(string CodigoCliente, EntityCliente cliente) {
            int IError;//variable para los errores de contpaq
            IError = Clientes.fBuscaCteProv(CodigoCliente);
            if (IError == 0)
            {
                IError = updateCliente(CodigoCliente, cliente);
            }
            else {
                IError = createCliente(CodigoCliente, cliente);                
            }
            SDK.MuestraError(IError, "createOrUpdateCliente");
            return IError;
        }

        public static int createCliente(string CodigoCliente, EntityCliente factura)
        {
            int IError;//variable para los errores de contpaq
            int IdCliente = 0;//variable del id de cliente            

            //asignacion del cliente con lo que se trae de la factura
            Clientes.tCteProv cliente = new Clientes.tCteProv();
            cliente.cCodigoCliente = CodigoCliente;
            cliente.cRazonSocial = factura.razonSocial;
            cliente.cRFC = factura.rfc;
            cliente.cCURP = factura.curp;
            cliente.cNombreMoneda = Convert.ToString("Peso Mexicano");
            cliente.cTipoCliente = Convert.ToInt32("1");
            cliente.cLimiteCreditoCliente = Convert.ToInt32("100000000");
            cliente.cBanVentaCredito = Convert.ToInt32("1");

            IError = Clientes.fAltaCteProv(ref IdCliente, ref cliente);
            SDK.MuestraError(IError, "fAltaCteProv");

            if (IError == 0)//en cero significa que el cliente se dio de alta correctamente...
            {
                //habilita la modificacion del cliente
                IError = Clientes.fEditaCteProv();
                SDK.MuestraError(IError, "fEditaCteProv");
            }

            if (IError == 0)
            {
                Console.WriteLine("EDICION DEL CLIENTE: {0} HABILITADA", CodigoCliente);
                //se agrega el correo al cliente
                IError = Clientes.fSetDatoCteProv("CEMAIL1", factura.correo);
                SDK.MuestraError(IError, "CEMAIL1");
            }
            if (IError == 0)
            {
                //modifica el dato de metodo pago
                IError = Clientes.fSetDatoCteProv("CMETODOPAG", factura.metodoPago);
                SDK.MuestraError(IError, "CMETODOPAG");
            }
            if (IError == 0)
            {
                //modifica el dato de uso cfdi
                IError = Clientes.fSetDatoCteProv("CUSOCFDI", factura.usoCFDI);
                SDK.MuestraError(IError, "CUSOCFDI");
            }
            if (IError == 0)
            {

                //(nuevo de inception)Tipo de entrega:0 = Correo electrónico 1 = Impresión 2 = Archivo en disco
                IError = Clientes.fSetDatoCteProv("CTIPOENTRE", "6");//6 no se porque asi estaba en inceptio
                SDK.MuestraError(IError, "CTIPOENTRE");
            }
            if (IError == 0)
            {
                //(nuevo de inception)Formato de entrega de CFD por omisión.0 = PDF 1 = XML
                IError = Clientes.fSetDatoCteProv("CFMTOENTRE", "3");//3 no se porque asi estaba en inceptio
                SDK.MuestraError(IError, "CFMTOENTRE");
            }
            if (IError == 0)
            {
                //(nuevo encontrado para factura version 4.0)Regimen fiscal segun bd cotpaq
                IError = Clientes.fSetDatoCteProv("CREGIMFISC", factura.regimenFiscal);//605 asalariados
                SDK.MuestraError(IError, "CREGIMFISC");
            }
            if (IError == 0)
            {
                //guarda los cambios en el cliente
                IError = Clientes.fGuardaCteProv();
                SDK.MuestraError(IError, "fGuardaCteProv");
            }
          
            
            SDK.MuestraError(IError, "createCliente");
            //regresa el resultado si es 0 todo bien cualquier otro se puede ver que fue con MuestraError del sdk
            return IError;

        }

        public static int updateCliente(string CodigoCliente, EntityCliente factura)
        {
            int IError;//variable para mostrar los errores

            //asignacion del cliente con lo que se trae de la factura
            Clientes.tCteProv cliente = new Clientes.tCteProv();
            cliente.cRazonSocial =factura.razonSocial;
            cliente.cRFC = factura.rfc;
            cliente.cCURP = factura.curp;
            cliente.cNombreMoneda = Convert.ToString("Peso Mexicano");
            cliente.cTipoCliente = Convert.ToInt32("1");
            cliente.cLimiteCreditoCliente = Convert.ToInt32("100000000");
            cliente.cBanVentaCredito = Convert.ToInt32("1");

            //se actualiza el cliente 
            IError = Clientes.fActualizaCteProv(CodigoCliente, ref cliente);
            SDK.MuestraError(IError, "fActualizaCteProv");
            if (IError == 0)//si es 0 se actualizo correctamente
            {
                Console.WriteLine("CLIENTE: ${0} ACTUALIZADO", CodigoCliente);
                //habilita la modificacion del cliente
                IError = Clientes.fEditaCteProv();
                SDK.MuestraError(IError, "fEditaCteProv");
            }
            if (IError == 0)
            {
                Console.WriteLine("EDICION DEL CLIENTE: ${0} HABILITADA", CodigoCliente);
                //se agrega el correo al cliente
                IError = Clientes.fSetDatoCteProv("CEMAIL1", factura.correo);
                SDK.MuestraError(IError, "CEMAIL1");
            }
            if (IError == 0)
            {
                //modifica el dato de metodo pago
                IError = Clientes.fSetDatoCteProv("CMETODOPAG", factura.metodoPago);
                SDK.MuestraError(IError, "CMETODOPAG");
            }
            if (IError == 0)
            {
                //modifica el dato de uso cfdi                    
                IError = Clientes.fSetDatoCteProv("CUSOCFDI", factura.usoCFDI);
                SDK.MuestraError(IError, "CUSOCFDI");
            }
            if (IError == 0)
            {
                //(nuevo de inception)Tipo de entrega:0 = Correo electrónico 1 = Impresión 2 = Archivo en disco
                IError = Clientes.fSetDatoCteProv("CTIPOENTRE", "6");//6 no se porque asi estaba en inceptio
                SDK.MuestraError(IError, "CTIPOENTRE");
            }
            if (IError == 0)
            {
                //(nuevo de inception)Formato de entrega de CFD por omisión.0 = PDF 1 = XML
                IError = Clientes.fSetDatoCteProv("CFMTOENTRE", "3");//3 no se porque asi estaba en inceptio
                SDK.MuestraError(IError, "CFMTOENTRE");
            }
            if (IError == 0)
            {
                //(nuevo encontrado para factura version 4.0)Regimen fiscal segun bd cotpaq
                IError = Clientes.fSetDatoCteProv("CREGIMFISC", factura.regimenFiscal);//605 asalariados
                SDK.MuestraError(IError, "CREGIMFISC");
            }
            if (IError == 0)
            {
                //guarda los cambios en el cliente
                IError = Clientes.fGuardaCteProv();
                SDK.MuestraError(IError, "fGuardaCteProv");
            }
               
            Console.WriteLine(SDK.MuestraErrorLogs(IError, "updateCliente"));

            //regresa el resultado si es 0 todo bien cualquier otro se puede ver que fue con MuestraError del sdk
            return IError;
        }

        public static int updatePublicoGeneral(string metodoPago) {
            int IError;

            IError = Clientes.fBuscaCteProv("1501");

            if (IError == 0) {
                IError = Clientes.fEditaCteProv();
                SDK.MuestraError(IError, "fEditaCteProv");
            }
            
            if (IError==0) {
                IError = Clientes.fSetDatoCteProv("CMETODOPAG", metodoPago);
                SDK.MuestraError(IError, "CMETODOPAG");
            }

            if (IError == 0)
            {
                //guarda los cambios en el cliente
                IError = Clientes.fGuardaCteProv();
                SDK.MuestraError(IError, "fGuardaCteProv");
            }

            SDK.MuestraError(IError, "updatePublicoGeneral");
            return IError;
        }
    }
}
