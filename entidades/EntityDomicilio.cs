using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace entidades
{
    [DataContract]
    public class EntityDomicilio
    {

        public EntityDomicilio(string calle,string numExt,string numInt,string colonia,string cp,string municipio,string estado,string pais) {
            this.calle = calle;
            this.numExt = numExt;
            this.numInt = numInt;
            this.colonia = colonia;
            this.cp = cp;
            this.municipio = municipio;
            this.estado = estado;
            this.pais = pais;
        }
        [DataMember]
        public string calle { get; set; }
        [DataMember]
        public string numExt { get; set; }
        [DataMember]
        public string numInt { get; set; }
        [DataMember]
        public string colonia { get; set; }
        [DataMember]
        public string cp { get; set; }
        [DataMember]
        public string municipio { get; set; }
        [DataMember]
        public string estado { get; set; }
        [DataMember]
        public string pais { get; set; }

    }
}
