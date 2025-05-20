using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace restApi.Entidad
{
    [DataContract]
    public class Documentos
    {
        [DataMember]
        public int idDocumento { get; set; }
        [DataMember]
        public string codigo_prod_serv { get; set; }
        [DataMember]
        public int unidades { get; set; }
        [DataMember]
        public double precio { get; set; }
        [DataMember]
        public double descuento { get; set; }

        [DataMember]
        public double precio_total { get; set; }//se agrega como base para comparar y redondear o ajustar en caso de que compac calcule el iva mal.....
    }
}
