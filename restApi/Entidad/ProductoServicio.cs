using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace restApi.Entidad
{
    [DataContract]
    public class ProductoServicio
    {
        [DataMember]
        public string codigo_prod_serv { get; set; }

        [DataMember]
        public string nombre_prod_serv { get; set; }

        [DataMember]
        public string precio { get; set; }

        [DataMember]
        public string iva { get; set; }
    }
}
