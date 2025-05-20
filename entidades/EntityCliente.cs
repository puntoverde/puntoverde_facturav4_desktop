using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace entidades
{
    [DataContract]
    public class EntityCliente
    {

        public EntityCliente(string rfc,string razonSocial,string curp,string correo,string metodoPago,string usoCFDI,string regimenFiscal) {
            this.rfc = rfc;
            this.razonSocial = razonSocial;
            this.curp = curp;
            this.correo = correo;
            this.metodoPago = metodoPago;
            this.usoCFDI = usoCFDI;
            this.regimenFiscal = regimenFiscal;
        }
        [DataMember]
        public string rfc { get; set; }//rfc del socio
        [DataMember]
        public string razonSocial { get; set; }//razon social del socio
        [DataMember]
        public string curp { get; set; }//curp del socio
        [DataMember]
        public string correo { get; set; }//correo del socio
        [DataMember]
        public string metodoPago { get; set; }//efectivo,tarjeta etc
        [DataMember]
        public string usoCFDI { get; set; }//G03, P10 etc
        [DataMember]
        public string regimenFiscal { get; set; }//605 personas asalariadas etc


    }
}
