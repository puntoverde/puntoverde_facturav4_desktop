using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace restApi.Entidad
{
    [DataContract]
    public class FacturaResponse
    {
        [DataMember]
        public double folio { get; set; }//folio del documento
        [DataMember]
        public string uuid { get; set; }//uuid del xml de compaq
        [DataMember]
        public int IError { get; set; }//y el codigo de error segun suceda

        [DataMember]
        public string IErrorMessage { get; set; }//es el mensaje por parte de compaq

        [DataMember]
        public int estado { get; set; }//es el estado en comercial[0,1,2,3,4]
    }
}
