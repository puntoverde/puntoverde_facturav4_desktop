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

    public class WCFCompaq : IWCFCompaq, IDisposable
    {
        
        public WCFCompaq()
        {
        }

        public void Dispose()
        {
           
        }

        #region facturacion
        public Message factura(Stream streamOfData,string cod_cliente)
        {
            WebOperationContext ctx = WebOperationContext.Current;

            //se crea objeto para serializar el json a una clase
            DataContractJsonSerializer jsonSerialize = new DataContractJsonSerializer(typeof(FacturaRequest));
            
            //serializa el stream al objeto factura
            FacturaRequest req = (FacturaRequest)jsonSerialize.ReadObject(streamOfData);
            FacturaResponse resp = new FacturaResponse();

            Dictionary < string, object> data= ComercialMain.facturarIndividual(cod_cliente,req.cliente,req.domicilio,req.movimientos,req.folio_pv,req.observaciones, req.fecha_pago);
            if (data != null)
            {
                resp.folio = (double)data["folio"];
                resp.uuid = (string)data["uuid"];
                resp.IError = (int)data["IError"];
                resp.IErrorMessage = (string)data["IErrorMessage"];
                resp.estado = (int)data["estado"];
                Console.WriteLine("RESPUESTA DE API FACTURA:{0}", resp);

                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return ctx.CreateJsonResponse<FacturaResponse>(resp);
            }

            else {

                ctx.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                return ctx.CreateJsonResponse(new { body="ocurrio un error por lo cual no se pudo guardar"});
            }

            

            /*if (IError == 0) {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return ctx.CreateJsonResponse<FacturaResponse>(resp);
            }
            else { 
                 ctx.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                 return ctx.CreateJsonResponse("fallo");
             }*/

            //Console.WriteLine(resp);

            //ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            //return ctx.CreateJsonResponse("HOLA PRUEBA");

            
            
        }

        public Message factura_publico_general(Stream streamOfData)
        {
            WebOperationContext ctx = WebOperationContext.Current;

            //se crea objeto para serializar el json a una clase
            DataContractJsonSerializer jsonSerialize = new DataContractJsonSerializer(typeof(FacturaRequestPG));

            //serializa el stream al objeto factura
            FacturaRequestPG req = (FacturaRequestPG)jsonSerialize.ReadObject(streamOfData);
            FacturaResponse resp = new FacturaResponse();

            Dictionary<string, object> data= ComercialMain.facturarPublicoGeneral(req.movimientos,req.folio_pv,req.observaciones,req.metodoPago,req.razonSocial,req.fecha_pago);
            resp.folio = (double)data["folio"];
            resp.uuid = (string)data["uuid"];
            resp.IError = (int)data["IError"];
            resp.IErrorMessage = (string)data["IErrorMessage"];
            resp.estado = (int)data["estado"];

            //Console.WriteLine("Error de Factura Individual:{0}",IError);

            Console.WriteLine(req);

            /*if (IError == 0) {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return ctx.CreateJsonResponse<FacturaResponse>(resp);
            }
            else { 
                 ctx.OutgoingResponse.StatusCode = HttpStatusCode.InternalServerError;
                 return ctx.CreateJsonResponse("fallo");
             }*/

            //Console.WriteLine(resp);

            //ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            //return ctx.CreateJsonResponse("HOLA PRUEBA");


            ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
            return ctx.CreateJsonResponse<FacturaResponse>(resp);
        }
        #endregion


        #region productos
        public Message get_productos()
        {
            WebOperationContext ctx = WebOperationContext.Current;
            try
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return ctx.CreateJsonResponse<List<ProductoServicio>>(ProdServModel.lst_prod_serv());
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.Message);
            }
        }

        public Message get_producto(string cod_producto)
        {
            WebOperationContext ctx = WebOperationContext.Current;
            try
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.OK;
                return ctx.CreateJsonResponse<ProductoServicio>(ProdServModel.getProducto(cod_producto));
            }
            catch (Exception ex)
            {
                ctx.OutgoingResponse.StatusCode = HttpStatusCode.PartialContent;
                return ctx.CreateJsonResponse(ex.Message);
            }
        }
        #endregion
    }
}