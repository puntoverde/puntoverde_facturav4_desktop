using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace restApi.Entidad
{
    [DataContract]
    public class ResponseMovimiento
    {
        [DataMember]
        public string id { get; set; }//id movimiento
        [DataMember]
        public string producto { get; set; }//nombre del producto
        [DataMember]
        public string cantidad { get; set; }//y el codigo de error segun suceda

        [DataMember]
        public string precio { get; set; }//precio unitario

        [DataMember]
        public string total { get; set; }//total precio unitario por la cantidad
    }
}
