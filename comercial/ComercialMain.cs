
using contpaqSDK;
using entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace comercial
{
    public class ComercialMain
    {

        public static int abrirEmpresa()
        {
            int IError;

            //RegistryKey keySistema = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\\Computación en Acción, SA CV\\CONTPAQ I COMERCIAL");
            //object lEntrada = keySistema.GetValue("DirectorioBase");

            //nos posicionamos en el directorio de comercial
            SDK.SetCurrentDirectory(@"C:\Program Files (x86)\Compac\COMERCIAL".Trim());
            //SDK.SetCurrentDirectory(lEntrada.ToString().Trim());

            //inicio de session en comercial
            SDK.fInicioSesionSDK("SUPERVISOR", "Admin20");

            //se abre la conexion con contabilidad
            SDK.fInicioSesionSDKCONTPAQi("SUPERVISOR", "");

            //indicamos el paquete que usaremos(comercial,adminpaq,facturacion)
            IError = SDK.fSetNombrePAQ("CONTPAQ I COMERCIAL".Trim());
            if (IError == 0)
            {
                Console.WriteLine("PAQUETE CORRECTAMENTE");
                //abre empresa
                 //IError = SDK.fAbreEmpresa(@"C:\Compac\Empresas\adPruebas".Trim());//adPruebas
                IError = SDK.fAbreEmpresa(@"C:\Compac\Empresas\adPUNTO_VERDE_2016".Trim());
                if (IError == 0)
                {
                    Console.WriteLine("EMPRESA ABIERTA...");
                }
                else { SDK.MuestraError(IError, "NO SE PUDO ABRIR EMPRESA"); }
            }
            else
            {
               SDK.MuestraError(IError, "NO SE INDICA EL PAQUETE CORRECTO");
            }

            return IError;
        }

        public static int cerrarEmpresa()
        {
            int IError;
            //cierra empresa
            IError = SDK.fCierraEmpresa();
            Console.WriteLine("CERRO EMPRESA");
            IError = SDK.fTerminaSDK();
            Console.WriteLine("TERMINA SDK");
            return IError;

        }

        #region facturacion
        public static Dictionary<string, object> facturarIndividual(String codCliente,EntityCliente cliente,EntityDomicilio domicilio,List<EntityMovimiento> movimientos,string folio_pv,string observaciones,string fecha_pago)
        {            
            Dictionary<string, object> response = new Dictionary<string, object>() { { "folio", 0 }, { "uuid", 0 }, { "IError", 0 },{ "IErrorMessage","" },{ "estado", 0} };

            int IError;//variable error
            double folio=0;//almacena folio del documenrto creado 
            int id_documento = 0;//id del documento creado
            string uuid = "";
            int estado = 0;//es el estado de la factura
            double folio_saldo = 0;
            int id_documento_saldo = 0;


            //paso 1 crea o actualiza el cliente
            IError = ClienteImplements.createOrUpdateCliente(codCliente, cliente);
                
            if (IError == 0)
            {
                //paso 2 se crea o actualiza el domicilio fiscal
                IError = DomicilioImplements.createOrUpdateDomicilio(codCliente, domicilio);
                estado = 1;//1 es cliente actualizado
            }
            if (IError == 0)
            {
                //paso 3 se crea el documento
                Dictionary<string, object> data = DocumentoImplements.crearDocumento(codCliente,folio_pv,observaciones);
                folio = (double)data["folio"];
                id_documento = (int)data["id_documento"];
                IError = (int)data["IError"];
                estado = 2;//2 es domicilo actualizado
            }
            if (IError == 0)
            {                
                //paso 4 se crean los movimientos                
                IError = MovimientoImplements.crearMovimientos(id_documento, movimientos);
                estado = 3;//3 es documento creado
            }
            if (IError == 0)
            {
                //paso 5 se salda el documento
                Dictionary<string, object> data= DocumentoImplements.saldarDocumento(codCliente, id_documento,folio, fecha_pago);
                folio_saldo = (double)data["folio"];
                id_documento_saldo = (int)data["id_documento"];
                IError = (int)data["IError"];
                estado = 4;//es movimientos agregados
            }
            if (IError == 0)
            {
                //paso 6 se timbra el documento
                IError= DocumentoImplements.timbrarDocumento(folio, "42018", "");
                estado = 5;//5 es documento saldado con exito 
            }
            if (IError == 0)
            {
                //paso 7 se obtiene el uuid del documento timbrado               
                Dictionary<string, object> data = DocumentoImplements.getuuid(folio, "42018", "");
                uuid = (string)data["uuid"];
                IError = (int)data["IError"];
                estado = 6;//6 es docuemnto se timbro con exito
            }
            if (IError == 0)
            {
                //paso 8 se genera los documentos xml y el pdf
                IError= DocumentoImplements.generarXMLPDF(folio, "42018", "");
                estado = 7;//7 es obtencion del  uuid correctamente
            }
            if (IError == 0)
            {
                //paso 9 se envia correo con el xml y el pdf
                
                IError= DocumentoImplements.EnviarCorreo(cliente.correo, folio);
                estado = 8;//8 es generacion de xmp y pdf 
            }

            if (IError == 0)
            {
                IError = DocumentoImplements.desbloqueaDocumento();
                estado = 9;//9 es envio de correo
            }

            Console.WriteLine(SDK.MuestraError(IError, "COMPAQ"));

            if (IError == 0)
            {

                response["folio"] = folio;
                response["uuid"] = uuid;
                response["IError"] = IError;
                response["IErrorMessage"] = SDK.MuestraErrorLogs(IError, "SDK");
                response["estado"] = estado;
                return response;
            }
            else
            {
                string mensaje_error = SDK.MuestraErrorLogs(IError, "SDK");
                //se elimina el documento 
                if (DocumentoImplements.deleteDocumento(id_documento) == 0) mensaje_error = mensaje_error + " - SE ELIMINO DOCUMENTO CON FOLIO:" + folio;
                else mensaje_error = mensaje_error + " - OCURRIO ERROR AL ELIMINAR EL DOCUMENTO CON FOLIO:" + folio;
                //se elimina el documento de saldo
                if (DocumentoImplements.deleteDocumentoSaldar(id_documento_saldo) == 0) mensaje_error = mensaje_error + " - SE ELIMINO DOCUMENTO DE SALDO CON FOLIO:" + folio_saldo;
                else mensaje_error = mensaje_error + " - OCURRIO ERROR AL ELIMINAR EL DOCUMENTO DE SALDO CON FOLIO:" + folio_saldo;

                response["folio"] = folio;
                response["uuid"] = uuid;
                response["IError"] = IError;
                response["IErrorMessage"] = mensaje_error;
                response["estado"] = estado;
                return response;
            }

        }
        
        public static Dictionary<string, object> facturarPublicoGeneral(List<EntityMovimiento> movimientos,string folio_pv,string observaciones,string formaPago,string razonSocial, string fecha_pago)
        {
            Dictionary<string, object> response = new Dictionary<string, object>() { { "folio", 0 }, { "uuid", 0 }, { "IError", 0 }, { "IErrorMessage", "" }, { "estado", 0 } };

            int IError;
            double folio = 0;
            int id_documento = 0;
            string CodigoCliente = "1501";//rfc publico en general
            string uuid = "";
            int estado = 0;
            double folio_saldo = 0;
            int id_documento_saldo = 0;

            //solo actualiza la forma de pago efectivo,tarjeta debito etc, el uso cfdi y el regimen lo toma del mismo publico en general
            IError = ClienteImplements.updatePublicoGeneral(formaPago);

            if (IError == 0)
            {
                //paso 3 se crea el documento
                Dictionary<string, object> data = DocumentoImplements.crearDocumento(CodigoCliente,folio_pv ,observaciones);
                folio = (double)data["folio"];
                id_documento = (int)data["id_documento"];
                IError = (int)data["IError"];
            }
            if (IError == 0)
            {
                IError = DocumentoImplements.ChangeRazonSocialDocumento(razonSocial);//id_documento,
            }
            if (IError == 0)
            {
                //paso 4 se crean los movimientos
                IError = MovimientoImplements.crearMovimientos(id_documento, movimientos);
                estado = 3;//se creo con exito el documento
            }
            if (IError == 0)
            {
                //paso 5 se salda el documento
                //XAXX010101000
                Dictionary<string, object> data  = DocumentoImplements.saldarDocumento("1501", id_documento, folio, fecha_pago);
                folio_saldo = (double)data["folio"];
                id_documento_saldo = (int)data["id_documento"];
                IError = (int)data["IError"];
                estado = 4;//se cargaron los movimientos
            }

            if (IError == 0)
            {
                //paso 6 se timbra el documento
                IError = DocumentoImplements.timbrarDocumento(folio, "42018", "");
                estado = 5;//se saldo con exito el documento
            }


            if (IError == 0)
            {
                //paso 7 se obtiene el uuid del documento timbrado
                Dictionary<string, object> data = DocumentoImplements.getuuid(folio, "42018", "");                
                uuid = (string)data["uuid"];
                IError = (int)data["IError"];
                estado = 6;//se timbro con exito el documento
            }
            if (IError == 0)
            {
                //paso 8 se genera los documentos xml y el pdf
                /////IError = DocumentoImplements.generarXMLPDF(folio, "42018", "");
            }

            if (IError == 0)
            {
                //paso 9 se envia correo con el xml y el pdf
                /////IError = DocumentoImplements.EnviarCorreo("facturacionpuntoverdeleon@gmail.com", folio);
            }

            if (IError == 0)
            {
                IError = DocumentoImplements.desbloqueaDocumento();
                estado = 7;//se obtiene el uuid de forma correcta
            }

            Console.WriteLine(SDK.MuestraError(IError, "COMPAQ PUBLICO GENERAL"));
            if (IError == 0)
            {

                response["folio"] = folio;
                response["uuid"] = uuid;
                response["IError"] = IError;
                response["IErrorMessage"] = SDK.MuestraErrorLogs(IError, "SDK");
                response["estado"] = estado;
                return response;
            }
            else
            {
                string mensaje_error= SDK.MuestraErrorLogs(IError, "SDK");
                //se elimina el documento 
                if (DocumentoImplements.deleteDocumento(id_documento)==0)mensaje_error=mensaje_error+" - SE ELIMINO DOCUMENTO CON FOLIO:"+folio;
                else mensaje_error = mensaje_error + " - OCURRIO ERROR AL ELIMINAR EL DOCUMENTO CON FOLIO:" + folio;
                //se elimina el documento de saldo
                if (DocumentoImplements.deleteDocumentoSaldar(id_documento_saldo)==0) mensaje_error = mensaje_error + " - SE ELIMINO DOCUMENTO DE SALDO CON FOLIO:" + folio_saldo;
                else mensaje_error = mensaje_error + " - OCURRIO ERROR AL ELIMINAR EL DOCUMENTO DE SALDO CON FOLIO:" + folio_saldo;

                response["folio"] = folio;
                response["uuid"] = uuid;
                response["IError"] = IError;
                response["IErrorMessage"] = mensaje_error;
                response["estado"] = estado;
                return response;
            }
        }
        #endregion

        #region 

        #endregion


    }
}
