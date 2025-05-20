namespace restApi
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.ServiceModel;
    using System.Linq;
    using System.ServiceModel.Web;
    using System.ServiceModel.Activation;
    using System.Collections.Specialized;
    using System.ServiceModel.Channels;

    using System.IO;
    using System.Runtime.Serialization.Json;
    using System.Data.SqlClient;
    using System.Configuration;
    //using Novacode;
    //using Aspose.Words;
    using System.Text.RegularExpressions;
    using System.Text;
    //using HtmlAgilityPack;
    //using Newtonsoft.Json;
    using restApi.Entidad;
    using contpaqSDK;
    using comercial;
    using entidades;
    using restApi.Modelo;

    public class WCFCompaqcopia : IWCFCompaq, IDisposable
    {
        
        public WCFCompaqcopia()
        {
        }

        public void Dispose()
        {
           
        }

        public Message factura(Stream streamOfData,string cod_cliente)
        {
            Console.WriteLine("Codigo Cliente:{0}", cod_cliente);
            WebOperationContext ctx = WebOperationContext.Current;
            /*try {
                //objeto con el cual se estructura el regreso
                FacturaResponse responseFactura = new FacturaResponse();
                //crea objeto para para serializar json aun objeto de clase
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(FacturaRequest));
                //serializa el stream al objeto factura
                FacturaRequest requestFactura = (FacturaRequest)ser.ReadObject(streamOfData);
                //asigna el codigo cliente al objeto factura este viene como parametro
                int IError=0;//variable para errores de comercial
                double folio = 0;//folio del documento
                int id_documento = 0;//id interno del documento                              
                string mensaje = "";//es para mensajes

                string CodigoCliente = requestFactura.codigo_cliente;



                        if (IError == 0)
                        {
                            EntityCliente cliente = new EntityCliente(
                                requestFactura.rfc,
                                requestFactura.razon_social,
                                requestFactura.curp,
                                requestFactura.correo,
                                requestFactura.metodo_pago,
                                requestFactura.uso_cfdi,
                                requestFactura.regimenFiscal
                                );
                            //paso 1 crea o actualiza el cliente
                            IError = ClienteImplements.createOrUpdateCliente(CodigoCliente, cliente);
                        }

                        if (IError == 0)
                        {
                    EntityDomicilio domicilio = new EntityDomicilio(
                        requestFactura.calle,
                        requestFactura.num_ext,
                        requestFactura.num_int,
                        requestFactura.colonia,
                        requestFactura.cp,
                        requestFactura.ciudad,
                        requestFactura.estado,
                        requestFactura.pais
                        );
                            //paso 2 se crea o actualiza el domicilio fiscal
                            IError = DomicilioImplements.createOrUpdateDomicilio(CodigoCliente, domicilio);
                        }

                        if (IError == 0)
                        {
                            //paso 3 se crea el documento
                            Dictionary<string, object> data = DocumentoImplements.crearDocumento(CodigoCliente, requestFactura.folio_pv,"observaciones");
                            folio = (double)data["folio"];
                            id_documento = (int)data["id_documento"];
                            IError = (int)data["IError"];
                        }

                        if (IError == 0)
                        {
                    List<EntityMovimiento> movimientos = new List<EntityMovimiento>();
                    foreach (Documentos mov in requestFactura.Documentos)
                    {
                        movimientos.Add(new EntityMovimiento(mov.codigo_prod_serv,mov.unidades,mov.precio,mov.descuento));
                    }
                    //paso 5 se crean los movimientos
                    IError = MovimientoImplements.crearMovimientos(id_documento, movimientos);
                  
                            

                        }

                        /////if (IError == 0)
                        /////{
                            /////Dictionary<string, object> data = DocumentoImplements.timbrarDocumento(folio, pago.conceptoPertenence, pago.getSerie);
                            /////IError = (int)data["IError"];
                            /////DAO.FacturaPagoDAO.updateFacturaUUID(2, folio.ToString(), (string)data["uuid"]);
                        /////}

                      


                if (IError == 0) { ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;}
                else { ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK; }                
                return ctx.CreateJsonResponse<FacturaResponse>(responseFactura);

            }//fin try
            catch (Exception ex) {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.ToString());
            }*/
            ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            return ctx.CreateJsonResponse("HOLA PRUEBA");
        }


        public Message factura_publico_general(Stream streamOfData)
        {
            /* WebOperationContext ctx = WebOperationContext.Current;
             try {
                 //clase para el reponse
                 ResponseFactura responseFactura = new ResponseFactura();
                 //crea objeto para para serializar json aun objeto de clase
                 DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Factura));
                 //serializa el stream al objeto factura
                 Factura factura = (Factura)ser.ReadObject(streamOfData);

                 //se actualiza busca para actualizar la forma de pago
                 if (Clientes.fBuscaCteProv("1501") == 0)
                 {
                     //se ahabilita la edicion del cliente    
                     Clientes.fEditaCteProv();
                     //modifica el dato de metodo pago
                     Console.Write("Metodo de pago  cliente->" + factura.forma_pago);
                     Clientes.fSetDatoCteProv("CMETODOPAG", factura.forma_pago);
                     //modifica el dato de uso cfdi
                     Clientes.fSetDatoCteProv("CUSOCFDI", factura.uso_cfdi);
                     //guarda los cambios en el cliente
                     Clientes.fGuardaCteProv();                    
                 }

                 string[] resp=FacturaModel.crearDocumento("1501", factura.Documentos,factura.observaciones,"PPD",factura.fecha_pago,factura.folio_pv);
                 responseFactura.folio = Convert.ToDouble(resp[0]);
                 responseFactura.IError = Convert.ToInt32(resp[1]);//agrega el error al codigo
                 responseFactura.estado = Convert.ToInt32(resp[2]);//estado en compaq
                 responseFactura.IErrorMessage = SDK.MuestraError(Convert.ToInt32(resp[1]), "->(Crear Documento y saldarlo)");
                 responseFactura.uuid = "-";//es temporal mientras sale 


                 if (responseFactura.IError == 0) { ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK; }
                 else { ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK; }
                 return ctx.CreateJsonResponse<ResponseFactura>(responseFactura);
             }
             catch (Exception ex) {
                 ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                 return ctx.CreateJsonResponse(ex.Message);
             }*/
            WebOperationContext ctx = WebOperationContext.Current;
            FacturaResponse responseFactura = new FacturaResponse();
            return ctx.CreateJsonResponse<FacturaResponse>(responseFactura);
        }

        public Message get_productos()
        {
            WebOperationContext ctx = WebOperationContext.Current;
            try {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return ctx.CreateJsonResponse<List<ProductoServicio>>(ProdServModel.lst_prod_serv());
            }
            catch (Exception ex) {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.Message);
            }
        }

        public Message get_producto(string cod_producto)
        {
            WebOperationContext ctx = WebOperationContext.Current;
            try {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return ctx.CreateJsonResponse<ProductoServicio>(ProdServModel.getProducto(cod_producto));
            }
            catch (Exception ex) {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.Message);
            }
        }

        public Message get_uuid(string folio)
        {
            WebOperationContext ctx = WebOperationContext.Current;
            try
            {
                string[] resp=FacturaModel.getUUID(Double.Parse(folio));
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return ctx.CreateJsonResponse(resp[1]);
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.Message);
            }
        }

        public Message cargarMovimientos(string folio)
        {
           
            WebOperationContext ctx = WebOperationContext.Current;
            try
            {
                FacturaResponse responseFactura = new FacturaResponse();
                return ctx.CreateJsonResponse<List<ResponseMovimiento>>(FacturaModel.getMovimientos(folio));
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.ToString());
            }
        }

        public Message agregarMovimiento(Stream streamOfData,string folio)
        {
            int IError = 0;
            WebOperationContext ctx = WebOperationContext.Current;
            try
            {
                //crea objeto para para serializar json aun objeto de clase
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Documentos));
                //serializa el stream al objeto factura
                Documentos movimiento = (Documentos)ser.ReadObject(streamOfData);

                IError = Documento.fBuscarDocumento("42018", "", folio);
                if (IError == 0)
                {
                    Console.WriteLine("documento encontrado...");
                    StringBuilder id_documento = new StringBuilder();
                    IError = Documento.fLeeDatoDocumento("CIDDOCUMENTO", id_documento, 10);
                    if (IError == 0)
                    {
                        if (movimiento.descuento == 0)
                        {
                            Console.WriteLine("se crea el movimiento");
                            IError = FacturaModel.crearMovimiento(movimiento.codigo_prod_serv, movimiento.unidades, movimiento.precio, movimiento.idDocumento, 0);
                        }
                        else
                        {
                            Console.WriteLine("se crea el movimiento con descuento");
                            IError = FacturaModel.crearMovimientoDescuento(movimiento.codigo_prod_serv, movimiento.unidades, movimiento.precio, movimiento.idDocumento, movimiento.descuento, Double.Parse(folio));
                        }
                    }
                    else { Console.WriteLine("no se puede obtener el id del documento"); }
                }
                else {
                    Console.WriteLine("no se encontro el documento");
                }

                
                if (IError == 0) { ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK; }
                else { ctx.OutgoingResponse.StatusCode = HttpStatusCode.Conflict; }
                return ctx.CreateJsonResponse(IError);//cero es todo esta bien

            }//fin try
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.ToString());
            }
        }

        public Message saldarDocumento(Stream streamOfData)
        {
            WebOperationContext ctx = WebOperationContext.Current;
            try
            {
                FacturaResponse responseFactura = new FacturaResponse();
                //crea objeto para para serializar json aun objeto de clase
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(SaldarFactura));
                //serializa el stream al objeto factura
                SaldarFactura saldar = (SaldarFactura)ser.ReadObject(streamOfData);
                //asigna el codigo cliente al objeto factura este viene como parametro

                int IError= FacturaModel.saldarDocumento(saldar.codigo_cliente,saldar.folio_compaq,saldar.fecha_pago);
                if (IError == 0)
                {
                    responseFactura.estado = 3;
                    responseFactura.IError = IError;
                    responseFactura.IErrorMessage = SDK.MuestraError(IError, "salda documento");
                }
                else {
                    responseFactura.estado = 2;
                    responseFactura.IError = IError;
                    responseFactura.IErrorMessage = SDK.MuestraError(IError, "salda documento");
                }
                
                return ctx.CreateJsonResponse<FacturaResponse>(responseFactura);
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.ToString());
            }
        }

        public Message timbrar(string folio)
        {
            WebOperationContext ctx = WebOperationContext.Current;
            int IError = 0;
            try
            {
                FacturaResponse responseFactura = new FacturaResponse();
                 IError= FacturaModel.timbrarDocumento(Double.Parse(folio));
                if (IError == 0)
                {
                    string[] resp = FacturaModel.getUUID(Double.Parse(folio));
                    ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                    return ctx.CreateJsonResponse<string>(resp[1]);
                }
                else {
                    ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                    return ctx.CreateJsonResponse("Error...");
                }
                
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.Message);
            }
        }

       
    }
}