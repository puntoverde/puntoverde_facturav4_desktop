using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;

namespace restApi
{
    [ServiceContract]
    public interface IValues 
    {
        [WebGet(UriTemplate = "values/{id}", ResponseFormat = WebMessageFormat.Json), CorsEnabled]
        string GetValue(string id);

        [WebInvoke(UriTemplate = "/values", Method = "POST", ResponseFormat = WebMessageFormat.Json), CorsEnabled]
        string AddValue(string value); 
    }
}
