using entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace restApi.Entidad
{
    [DataContract]
    public class FacturaRequestPG
    {
       
        [DataMember]
        public List<EntityMovimiento> movimientos { get; set; }

        //datos de documento
        //[DataMember]
        //public string forma_pago { get; set; }//pue o ppe

        [DataMember]
        public string metodoPago { get; set; }//forma de pago (efectivo,credito etc)

        [DataMember]
        public string razonSocial { get; set; }//razon social para publico en general

        [DataMember]
        public string fecha_pago { get; set; }//es la fecha de pago se usa en reportepagos web toma la fecha del pago para el documento de saldo
        
        [DataMember]
        public string folio_pv { get; set; }//es el folio del pago que esta en el sistema de pv...

        [DataMember]
        public string observaciones { get; set; }//son las observaciones del documento        

        


        public override string ToString()
        {
            return String.Format(@"
razon_social:{0},
metodo_pago:{1},
-----********-----
fecha pago:{2},
folio pago pv:{3},
observaciones:{4}
-----********-----
movimientos:{5}"
,razonSocial,metodoPago,fecha_pago,folio_pv,observaciones,string.Join(", ", movimientos));
        }


    }
}
