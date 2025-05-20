namespace restApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;
    using System.Threading.Tasks;

    public class CustomMapper : WebContentTypeMapper
    {
        public override WebContentFormat GetMessageFormatForContentType(string contentType)
        {
            return WebContentFormat.Raw;
        }

        public static Binding GetBinding()
        {
            var webBinding = new WebHttpBinding();
            webBinding.MaxBufferPoolSize = int.MaxValue;
            webBinding.MaxBufferSize = int.MaxValue;
            webBinding.MaxReceivedMessageSize = int.MaxValue;

            if (System.Configuration.ConfigurationManager.AppSettings["baseaddress"].Contains("https:"))
            {
                webBinding.Security.Mode = WebHttpSecurityMode.Transport;
                webBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            }

            CustomBinding result = new CustomBinding(webBinding);
            WebMessageEncodingBindingElement webMEBE = result.Elements.Find<WebMessageEncodingBindingElement>();
            webMEBE.ContentTypeMapper = new CustomMapper();
            return result;
        }
    }
}
