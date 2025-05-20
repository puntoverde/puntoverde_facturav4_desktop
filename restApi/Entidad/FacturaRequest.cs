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
    public class FacturaRequest
    {

        [DataMember]
        public EntityCliente cliente { get; set; }
        [DataMember]
        public EntityDomicilio domicilio { get; set; }
        [DataMember]
        public List<EntityMovimiento> movimientos { get; set; }

        //datos de documento
        //[DataMember]
        //public string forma_pago { get; set; }//forma de pago (efectivo,credito etc)

        //[DataMember]
        //public string metodo_pago { get; set; }//pue o ppe

        [DataMember]
        public string fecha_pago { get; set; }//es la fecha de pago se usa en reportepagos web toma la fecha del pago para el documento de saldo
        
        [DataMember]
        public string folio_pv { get; set; }//es el folio del pago que esta en el sistema de pv...

        [DataMember]
        public string observaciones { get; set; }//son las observaciones del documento        

        


        public override string ToString()
        {
            return String.Format(@"rfc:{0},
razon_social:{1},
curp:{2},
correo:{3},
metodo_pago:{4},
uso_cfdi:{5},
regimen_fiscal:{6}
-----********-----
calle:{7},
numero_ext:{8},
numero_int:{9},
colonia:{10},
codigo_postal:{11},
municipio:{12},
estado:{13},
pais:{14}
-----********-----
fecha pago:{15},
folio pago pv:{16},
observaciones:{17}
-----********-----
movimientos:{18}"
, cliente.rfc,cliente.razonSocial,cliente.curp,cliente.correo,cliente.metodoPago,cliente.usoCFDI,cliente.regimenFiscal,
domicilio.calle,domicilio.numExt,domicilio.numInt,domicilio.colonia,domicilio.cp,domicilio.municipio,domicilio.estado,domicilio.estado,
fecha_pago,folio_pv,observaciones,
string.Join(", ", movimientos));
        }


    }
}
