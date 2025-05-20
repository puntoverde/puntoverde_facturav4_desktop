using contpaqSDK;
using entidades;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;

namespace comercial
{
    public class DocumentoImplements
    {
        public static Dictionary<string, object> crearDocumento(string CodigoCliente,string idPagoPV,string observaciones)
        {
            //dictionary es la respusta del metodo 
            Dictionary<string, object> response = new Dictionary<string, object>() { {"id_documento",0 },{"folio",0 },{"IError",0 }};
            //variable para errores de contpaq
            int IError;
            //variable para el id documento generado
            int idDocumento = 0;
            //concepto al cual se agregara
            string idConcepto = "42018";
            //serie del documento
            //StringBuilder serie = new StringBuilder("AC",12);
            StringBuilder serie = new StringBuilder(12);//no seingresa serie no se manega en el concepto 42018
            //folio del documento
            double ifolio = 0;

            //obtiene el folio siguiente
            IError = Documento.fSiguienteFolio(idConcepto, serie, ref ifolio);
            SDK.MuestraError(IError, "fSiguienteFolio");
            if (IError == 0)
            {
                Documento.tDocumento docu = new Documento.tDocumento();
                docu.aCodConcepto = idConcepto;
                //docu.aSerie = "AC";
                docu.aSerie = serie.ToString();
                docu.aFolio = ifolio;
                docu.aFecha = DateTime.Today.ToString("MM/dd/yyyy");
                docu.aCodigoCteProv = CodigoCliente;
                docu.aCodigoAgente = "(Ninguno)";
                docu.aSistemaOrigen = 1;
                docu.aTipoCambio = 1;
                docu.aNumMoneda = 1;
                docu.aImporte = 0;
                docu.aDescuentoDoc1 = 0;
                docu.aDescuentoDoc2 = 0;
                docu.aAfecta = 0;

                IError = Documento.fAltaDocumento(ref idDocumento, ref docu);
                SDK.MuestraError(IError, "fAltaDocumento");
            }
            if (IError == 0)
            {
                //se asigna el iddocumento al array para regresarlo esto indica que se creo un documento
                response["id_documento"] = idDocumento;
                //se asigna el folio al array para regresarlo esto indica que se creo un documento
                response["folio"] = ifolio;

                Console.WriteLine("SE CREA CON EXITO EL DOCUMENTO CON FOLIO: ${0}", ifolio.ToString());

                //se agrega descripcion al documento
                Documento.fEditarDocumento();//se ahabilita la edicion del documento
                SDK.MuestraError(IError, "fEditarDocumento");
            }
            if (IError == 0)
            {
                //se agrega el metdoo pago al doc ya sea pue o ppd --->Documento.fSetDatoDocumento("CCANTPARCI", "PUE")
                Documento.fSetDatoDocumento("CCANTPARCI", "1");
                SDK.MuestraError(IError, "CCANTPARCI");
            }
            if (IError == 0)
            {
                //se agrega la descripcion
                Documento.fSetDatoDocumento("COBSERVACIONES", observaciones);
                SDK.MuestraError(IError, "COBSERVACIONES");
            }
            if (IError == 0)
            {
                //se integrara el id de pago del pv
                Documento.fSetDatoDocumento("CTEXTOEXTRA1", idPagoPV);
                SDK.MuestraError(IError, "CTEXTOEXTRA1");
            }
            if (IError == 0)
            {
                //guarda los cambios en el documento
                Documento.fGuardaDocumento();
                SDK.MuestraError(IError, "fGuardaDocumento");
            }            

            response["IError"] = IError;
            SDK.MuestraError(IError, "fAltaDocumento");
            return response;
        }

        public static Dictionary<string, object> saldarDocumento(string CodigoCliente, int idDocumento, double folio_factura, string fecha_pago) {

            Dictionary<string, object> response = new Dictionary<string, object>() { { "id_documento", 0 }, { "folio", 0 }, { "IError", 0 } };

            int IError;//variable para los errores       
            StringBuilder serie = new StringBuilder("AC", 12);//serie de documentos de saldo
            string codConcepto = "13";//es fijo es donde van los docuemntos de saldo            
            double folio = 0;
            int idDocumentoSaldo = 0;//es para almacenar el id de documento del documento de saldo
            
            double total = 0;//se almacenara el total del doc recuperado de compac
            double subtotal = 0;
            double iva = 0;

            //datos del docuemtno de saldo
            double total_abono = 0;
            double subtotal_abono = 0;
            double iva_abono = 0;

            //fecha de saldo es la fecha en la que se pago ejemplo se pago el 01-mayo pero se faturo hasta el 28-mayo
            DateTime datetime;//es temporal solo para validar la fecha
            string fechaSaldar = DateTime.TryParse(fecha_pago, out datetime) ? datetime.ToString("MM/dd/yyyy") : DateTime.Today.ToString("MM/dd/yyyy");

            Documento.tLlaveDoc factura = new Documento.tLlaveDoc();//llave para la factura 
            Documento.tLlaveDoc pago = new Documento.tLlaveDoc();//llave para el doc de saldo o abono

            //SE RECUPERA EL DOCUMENTO DE LA FACTURA PARA POSICIONARSE SOBRE EL
            IError = Documento.fBuscarIdDocumento(idDocumento);
            SDK.MuestraError(IError, "fBuscarIdDocumento");
         

            if (IError == 0)
            {

                //recupera el total de la factura
                StringBuilder c_total = new StringBuilder(12);
                Documento.fLeeDatoDocumento("CTOTAL", c_total, 30);
                if (IError == 0) total = Double.Parse(c_total.ToString());
                SDK.MuestraError(IError, "CTOTAL");
                Console.WriteLine("RECUPERA EL TOTAL DE LA FACTURA QUE ES:{0} PARA SALDAR", total);
            }
            if (IError == 0)
            {

                //EN ESTA INSTACIA YA SE CUENTA CON EL TOTAL DE LA FACTURA SE REALIZA LA OBTENCION DEL SUBTOTAL E IVA
                subtotal = getSubtotal(total);//SE OBTIENE EL SUBTOTAL
                iva = getIva(total);//SE OBTIENE EL IVA
                //LIBERA LA FACTURA PARA QUE NO ESTE EN MEMORIA
                IError=Documento.fDesbloqueaDocumento();
                SDK.MuestraError(IError, "fDesbloqueaDocumento");
            }

            //inicio de documento de saldo igual esto puede  posicionar en el nuevo documento
            if (IError == 0) {
                IError = Documento.fSiguienteFolio(codConcepto, serie, ref folio);
                SDK.MuestraError(IError, "fSiguienteFolio");
            }

            if (IError == 0)
            {
                Documento.tDocumento lDocto = new Documento.tDocumento();                

                lDocto.aCodConcepto = codConcepto;
                lDocto.aCodigoCteProv = CodigoCliente; ;
                lDocto.aFecha = fechaSaldar;
                lDocto.aFolio = folio;
                lDocto.aImporte = subtotal;//es el subtotal compaq suma el iva
                lDocto.aNumMoneda = 1;
                lDocto.aTipoCambio = 1;
                lDocto.aSerie = serie.ToString();
                lDocto.aSistemaOrigen = 6;//indica que se realizo xon una aplicacion externa

                IError = Documento.fAltaDocumentoCargoAbono(ref lDocto);
                SDK.MuestraError(IError, "fAltaDocumentoCargoAbono");
            }

            if (IError == 0)
            {
                //SE OBTIENE EL ID DEL DOCUMENTO DE SALDO
                StringBuilder c_id_saldo = new StringBuilder(12);//se almacena el total de el doc de abono 
                IError = Documento.fLeeDatoDocumento("CIDDOCUMENTO", c_id_saldo, 30);
                if (IError == 0) idDocumentoSaldo = Int32.Parse(c_id_saldo.ToString());
                SDK.MuestraError(IError, "fLeeDatoDocumento");
            }

            if (IError == 0)
            {
                //SE OBTIENE EL TOTAL DEL DOCUMENTO DE SALDO
                StringBuilder c_total_abono = new StringBuilder(12);//se almacena el total de el doc de abono 
                IError = Documento.fLeeDatoDocumento("CTOTAL", c_total_abono, 30);
                if (IError == 0) total_abono = Double.Parse(c_total_abono.ToString());
                SDK.MuestraError(IError, "fLeeDatoDocumento");
            }
           
            //SI NO ES IGUAL INICIA MODIFICACION DEL DOCUMENTO 
            if (IError == 0 && total_abono != total) 
            {
                if (IError == 0)
                {
                    //SE OBTIENE EL SUBTOTAL DEL DOCUMENTO DE SALDO
                    StringBuilder c_subtotal_abono = new StringBuilder(12);//se almacena el total de el doc de abono 
                    IError = Documento.fLeeDatoDocumento("CNETO", c_subtotal_abono, 30);
                    if (IError == 0) subtotal_abono = Double.Parse(c_subtotal_abono.ToString());
                    SDK.MuestraError(IError, "fLeeDatoDocumento");
                }

                if (IError == 0)
                {
                    //SE OBTIENE EL IVA DEL DOCUMENTO DE SALDO
                    StringBuilder c_iva_abono = new StringBuilder(12);//se almacena el total de el doc de abono 
                    IError = Documento.fLeeDatoDocumento("CIMPUESTO1", c_iva_abono, 30);
                    if (IError == 0) iva_abono = Double.Parse(c_iva_abono.ToString());
                    SDK.MuestraError(IError, "fLeeDatoDocumento");
                }

                if (IError == 0)
                {
                    IError=Documento.fEditarDocumento();
                    SDK.MuestraError(IError, "fEditarDocumento");
                }
                //NO HAY ERROR Y EL SUBTOTAL ES DIFERENTE AL SUBTOTAL DE COMERCIAL
                if (IError == 0 && subtotal!=subtotal_abono)
                {
                    IError = Documento.fSetDatoDocumento("CNETO", subtotal.ToString());
                    SDK.MuestraError(IError, "fSetDatoDocumento");
                    Console.WriteLine("--SE CAMBIA SUBTOTAL DE COMERCIAL:{0} POR SUBTOTAL:{1}",subtotal_abono,subtotal);
                }
                //NO HAY ERROR Y EL SUBTOTAL ES DIFERENTE AL SUBTOTAL DE COMERCIAL
                if (IError == 0 && iva != iva_abono)
                {
                    IError = Documento.fSetDatoDocumento("CIMPUESTO1", iva.ToString());
                    SDK.MuestraError(IError, "fSetDatoDocumento");
                    Console.WriteLine("--SE CAMBIA IVA DE COMERCIAL:{0} POR IVA:{1}", iva_abono, iva);
                }
                if (IError == 0)
                { 
                    IError=Documento.fSetDatoDocumento("CTOTAL", total.ToString());
                    SDK.MuestraError(IError, "fSetDatoDocumento");
                    Console.WriteLine("--SE CAMBIA TOTAL DE COMERCIAL:{0} POR TOTAL:{1}", total_abono, total);
                }
                if (IError == 0) 
                { 
                    IError=Documento.fGuardaDocumento();
                    SDK.MuestraError(IError, "fGuardaDocumento");
                }

            }

            if (IError == 0)
            {
                factura.aCodConcepto = "42018";//es el codigo del concepto de los cargos y es fijo
                factura.aSerie = "";//no tiene serie
                factura.aFolio = folio_factura;//es el folio del otro documento que se dio de alta

                pago.aCodConcepto = codConcepto;//es el concepto 13 que es el documento que se acaba crear
                pago.aSerie = serie.ToString();//es serie AC
                pago.aFolio = folio;//es el folio de este documento que es el de saldo o abono

                //importe saldar es el total ya con iva
                IError = Documento.fSaldarDocumento(ref factura, ref pago, total, 1, datetime.ToString("MM/dd/yyyy"));
                
            }

            if (IError == 0)
            {
                IError = Documento.fDesbloqueaDocumento();
                SDK.MuestraError(IError, "fDesbloqueaDocumento");
            }

            //especial si ocurrio un error al momento de saldar 
            if (IError > 0)
            {
                //IErrorSaldar = Documento.fBuscarDocumento(codConcepto, serie.ToString(), folio.ToString());
                Documento.fBorraDocumento();//se borra documento el el cual esta posicionado segun yo es el de saldo
                //IError=Documento.fBuscarDocumento("42018", "", folio_factura.ToString());//vuelve a posicionar en la factura
            }

            response["id_documento"] = idDocumentoSaldo;
            response["folio"] = folio;
            response["IError"] = IError;
            return response;
        }
        
        public static /*Dictionary<string, object>*/ int timbrarDocumento(double folio,string codConcepto,string serie)
        {
            Dictionary<string, object> response = new Dictionary<string, object>() {{ "uuid", "" }, { "IError", 0 } };
            Console.WriteLine("INICIA TIMBRADO DEL DOCUEMNTO CON FOLIO:{0}",folio);
            int IError=0;
            //string password = "PuntoV14";//contraseña de timbres de prueba---pasword lux:  12345678a
            string password = "PuntOVL81";//contraseña de timbres de prueba---pasword lux:  12345678a
            StringBuilder uuid = new StringBuilder(40);//almacena el uuid        

            //IError = SDK.fInicializaLicenseInfo(0);//inicia la busqueda de la licencia si realmente es veridica
            //SDK.MuestraError(IError, "fInicializaLicenseInfo");
            if (IError == 0)
            {
                IError = Documento.fEmitirDocumento(codConcepto, serie, folio, password, "");
                SDK.MuestraError(IError, "fEmitirDocumento");
            }        

            return IError;
        }

        public static Dictionary<string, object> getuuid(double folio, string codConcepto, string serie)
        {
            Dictionary<string, object> response = new Dictionary<string, object>() { { "uuid", "" }, { "IError", 0 } };           
            int IError;
            StringBuilder uuid = new StringBuilder(40);//almacena el uuid
            Console.WriteLine("INICIA OBTENCION DEL UUID DEL DOCUENTO CON FOLIO:{0}",folio);
            IError = Documento.fDocumentoUUID(new StringBuilder(codConcepto), new StringBuilder(serie), folio, uuid);
            SDK.MuestraError(IError, "fDocumentoUUID");
            response["uuid"] = uuid.ToString();
            response["IError"] = IError;
            return response;
        }

        public static int generarXMLPDF(double folio, string codConcepto, string serie)
        {                        
            string plantillaPDF = @"C:\Compac\Empresas\Reportes\Formatos Digitales\Facturav4.0_PUNTO_VERDE_2022.rdl";
            int IError;          
            Console.WriteLine("INICIA GENERACION DEL PDF Y XML DE LA FACTURA CON FOLIO:{0}",folio);
            IError = Documento.fEntregEnDiscoXML(codConcepto, serie, folio, 0, plantillaPDF);
            IError = Documento.fEntregEnDiscoXML(codConcepto, serie, folio, 1, plantillaPDF);
            SDK.MuestraError(IError, "fDocumentoUUID");
            return IError;
        }

        public static int EnviarCorreo(string correo,double folio)
        {
            try
            {
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    UseDefaultCredentials=false,
                    Credentials = new NetworkCredential("facturacionpuntoverdeleon@gmail.com", "gtyakgyttfkvempd"),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("facturacionpuntoverdeleon@gmail.com"),
                    Subject = "Factura Punto Verde",
                    Body = "<h1>Comprobante fiscal factura punto verde</h1>",
                    IsBodyHtml = true
                };
                string filePDF = @"C:\Compac\Empresas\adPUNTO_VERDE_2016\XML_SDK\"+folio+".pdf";
                string fileXML = @"C:\Compac\Empresas\adPUNTO_VERDE_2016\XML_SDK\"+folio+".xml";
                var attachmentPDF = new Attachment(filePDF, MediaTypeNames.Application.Pdf);
                var attachmentXML = new Attachment(fileXML, MediaTypeNames.Application.Octet);

                mailMessage.Attachments.Add(attachmentPDF);
                mailMessage.Attachments.Add(attachmentXML);

                mailMessage.To.Add(correo);

                smtpClient.Send(mailMessage);
                Console.WriteLine("SE ENVIO CORREO A:{0}",correo);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR AL ENVIAR CORREO");
                Console.WriteLine(ex.Message);
                return 0;
            }
        }

        public static int desbloqueaDocumento()
        {
            //SE LIBERA EL DOCUEMENTO QUE ESTE EN MEMORIA
            int IError;            
            IError=Documento.fDesbloqueaDocumento();
            SDK.MuestraError(IError, "fDesbloqueaDocumento");

            return IError;
        }

        public static int ChangeRazonSocialDocumento(string razonSocial,int idDocumento=0)
        {
            int IError=0;

            if (idDocumento > 0) {
                IError = Documento.fBuscarIdDocumento(idDocumento);
                SDK.MuestraError(IError, "fBuscarIdDocumento");
            }            

            if (IError == 0)
            {
                IError = Documento.fEditarDocumento();
                SDK.MuestraError(IError, "fEditarDocumento");
            }
            if (IError == 0)
            {
                IError = Documento.fSetDatoDocumento("CRAZONSOCIAL", razonSocial);
                SDK.MuestraError(IError, "fSetDatoDocumento");
            }
            if (IError == 0)
            {
                IError = Documento.fGuardaDocumento();
                SDK.MuestraError(IError, "fGuardaDocumento");
            }

            SDK.MuestraError(IError, "ChangeRazonSocialDocumento");
            return IError;
        }

        public static int deleteDocumento(int idDocumento)
        {
            Console.WriteLine("INICIA ELIMACION DEL DOCUMENTO");

            if (idDocumento > 0)
            {
                int IError;
                IError = Documento.fBuscarIdDocumento(idDocumento);
                SDK.MuestraError(IError, "fBuscarIdDocumento");

                if (IError == 0)
                {
                    IError = Documento.fBorraDocumento();
                    SDK.MuestraError(IError, "fEditarDocumento");
                }

                return IError;
            }

            else return 1;

            
        }

        public static int deleteDocumentoSaldar(int idDocumento)
        {
            Console.WriteLine("INICIA ELIMACION DEL DOCUMENTO DE SALDO");

            if (idDocumento > 0)
            {
                int IError;
                IError = Documento.fBuscarIdDocumento(idDocumento);
                SDK.MuestraError(IError, "fBuscarIdDocumento");

                if (IError == 0)
                {
                    IError = Documento.fBorraDocumento();
                    SDK.MuestraError(IError, "fEditarDocumento");
                }

                return IError;
            }

            else return 1;
        }

        public static double getSubtotal(double total)
        {
            return Math.Round(Math.Truncate((total / 1.16) * 1000) / 1000, 2);
        }

        public static double getIva(double total)
        {
            /*var sub_total = Math.Truncate((total / 1.16) * 1000) / 1000;
            return Math.Round(Math.Truncate((sub_total * .16) * 1000) / 1000, 2, MidpointRounding.AwayFromZero);*/
            //var sub_total =Math.Truncate((total / 1.16) * 1000) / 1000;
            var sub_total = Math.Round(Math.Truncate((total / 1.16) * 1000) / 1000, 2);
            //return Math.Round(Math.Truncate((sub_total * .16) * 1000) / 1000,2,MidpointRounding.AwayFromZero);
            return total - sub_total;
        }

    }
}
