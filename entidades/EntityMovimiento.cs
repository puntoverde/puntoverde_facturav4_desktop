using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace entidades
{
    [DataContract]
    public class EntityMovimiento
    {

        public EntityMovimiento(string codProducto,int unidades,double importe,double descuento) {
            this.codProducto = codProducto;
            this.unidades = unidades;
            this.importe = importe;
            this.descuento = descuento;
        }
        [DataMember]
        public string idMovimiento { get; set; }
        [DataMember]
        public string codProducto { get; set; }
        [DataMember]
        public int unidades { get; set; }
        [DataMember]
        public double importe { get; set; }
        [DataMember]
        public double descuento { get; set; }

        public override string ToString()
        {
            return string.Format(@"
id:{0},
codigo:{1},
unidades:{2},
importe:{3},
descuento:{4}
_____________________", 
                idMovimiento,codProducto, unidades, importe, descuento);
        }

    }
}
