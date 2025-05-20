using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace restApi.Entidad
{
    [DataContract]
    public class SaldarFactura
    {
        [DataMember]
        public int idCliente { get; set; }
        [DataMember]
        public string codigo_cliente { get; set; }        
        [DataMember]
        public double monto_total_factura { get; set; }
        [DataMember]
        public double monto_subtotal_factura { get; set; }
        [DataMember]
        public string fecha_pago { get; set; }//es la fecha de pago se usa en reportepagos web toma la fecha del pago para el documento de saldo
        [DataMember]
        public double folio_compaq { get; set; }//es el folio de el documento de compaq guardado en local

    }
}
