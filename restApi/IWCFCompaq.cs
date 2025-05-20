namespace restApi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Web;
    using System.Text;
    using System.Threading.Tasks;

    [ServiceContract]
    public interface IWCFCompaq
    {
       
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/factura/{cod_cliente}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare), CorsEnabled]
        Message factura(Stream streamOfData,string cod_cliente);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/factura_publico_general", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare), CorsEnabled]
        Message factura_publico_general(Stream streamOfData);

        [OperationContract]
        [WebGet(UriTemplate = "/api/productos", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare), CorsEnabled]
        Message get_productos();

        
        [OperationContract]
        [WebGet(UriTemplate = "/api/productos/{cod_producto}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare), CorsEnabled]
        Message get_producto(string cod_producto);

        /*
        [OperationContract]
        [WebGet(UriTemplate = "/api/get-movimientos/{folio}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare), CorsEnabled]
        Message cargarMovimientos(string folio);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/add-movimiento/{folio}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare), CorsEnabled]
        Message agregarMovimiento(Stream streamOfData,string folio);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/saldar", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare), CorsEnabled]
        Message saldarDocumento(Stream streamOfData);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "/api/timbrar/{folio}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare), CorsEnabled]
        Message timbrar(string folio);

        [OperationContract]
        [WebGet(UriTemplate = "/api/get-uuid/{folio}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare), CorsEnabled]
        Message get_uuid(string folio);*/

    }
}
