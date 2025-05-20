using entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using contpaqSDK;

namespace comercial
{
    public class MovimientoImplements
    {

        public static int crearMovimientos(int idDocumento, List<EntityMovimiento> movimientos) {
            
            int IError=0;
            foreach (EntityMovimiento movimiento in movimientos)
            {
                if (movimiento.descuento == 0)
                {
                    //0= 0 + (0 si el movimeinto salio bien)
                    IError=IError+ crearMovimiento(idDocumento, movimiento);
                }
                else {
                    //0= 0 + (0 si el movimeinto salio bien)
                    IError = IError + crearMovimientoDescuento(idDocumento, movimiento);
                }              
            }
            SDK.MuestraError(IError, "crearMovimientos");
            return IError;
        }

        public static int crearMovimiento(int id_documento,EntityMovimiento mov)
        {
            
            int IError;
            int idMovimiento = 0;
            double totalMovComercial = 0;// se almacena el total del movimiento que este en el comercial
            double subtotalMovComercial = 0;// se almacena el subtotal del movimiento de comercial
            double ivaMovComercial = 0;// se almacena el iva del movimiento de comercial

            //se trunca a 3 decimales y se redonde a 2
            //nota el por mil en el truncate es para 3 y entre mil para volver a poner el decimal
            double subtotal = getSubtotal(mov.importe);//obtiene el subtotal
            double iva = getIva(mov.importe);
            Console.WriteLine("***SUBTOTAL:{0}", subtotal);
            Console.WriteLine("***IVA:{0}", iva);

            Movimiento.tMovimiento movimiento = new Movimiento.tMovimiento();
            movimiento.aCodAlmacen = "1";//es fijo
            movimiento.aConsecutivo = 1;//es fijo
            movimiento.aCodProdSer = mov.codProducto;
            movimiento.aUnidades = mov.unidades;
            movimiento.aPrecio = subtotal;//se obtiene el subtotal                       

            IError = Movimiento.fAltaMovimiento(id_documento, ref idMovimiento, ref movimiento);
            SDK.MuestraError(IError, "fAltaMovimiento");
            
            if (IError == 0)
            {
                Console.WriteLine("MOVIMIENTO DADO DE ALTA CON ID:{0}",idMovimiento);
                StringBuilder total_movimiento = new StringBuilder(12);
                IError = Movimiento.fLeeDatoMovimiento("CTOTAL", total_movimiento, 10);
                SDK.MuestraError(IError, "CTOTAL");
                if (IError == 0) totalMovComercial = Double.Parse(total_movimiento.ToString());
                Console.WriteLine("***TOTAL DE MOVIMIENTO EN COMERCIAL:{0}", total_movimiento);
            }

            //si el total de compac no es igual al total del movimiento
            if(IError==0 && totalMovComercial!= mov.importe)
            {
                Console.WriteLine("***INICIA PROCESO DE AJUSTE EN EL MOVIMIENTO TOTAL:{0} Y TOTAL COMERCIAL:{1}",mov.importe,totalMovComercial);                           
                //se recupera el subtotal del comercial 
                if (IError == 0) {
                    StringBuilder subtotal_movimiento = new StringBuilder(12);
                    IError = Movimiento.fLeeDatoMovimiento("CNETO", subtotal_movimiento, 10);
                    if (IError == 0) subtotalMovComercial = Double.Parse(subtotal_movimiento.ToString());
                    SDK.MuestraError(IError, "CNETO");
                    Console.WriteLine("***SUBTOTAL DE MOVIMIENTO EN COMERCIAL:{0}", subtotal_movimiento);
                }
                //se recupera el iva del comercial 
                if (IError == 0)
                {
                    StringBuilder iva_movimiento = new StringBuilder(12);
                    IError = Movimiento.fLeeDatoMovimiento("CIMPUESTO1", iva_movimiento, 10);
                    if (IError == 0) ivaMovComercial = Double.Parse(iva_movimiento.ToString());
                    SDK.MuestraError(IError, "CIMPUESTO1");
                    Console.WriteLine("***IVA EN COMERCIAL:{0}", iva_movimiento);
                }
                
                //se habilita la edicion
                if (IError == 0)
                {
                    Console.WriteLine("***INICIA EDICION DEL MOVIMIENTO");
                    IError = Movimiento.fEditarMovimiento();
                    SDK.MuestraError(IError, "fEditarMovimiento");
                }
                //NO HAY ERROR Y EL SUBTOTAL DE COMERCIAL ES DISTINTO AL QUE SE PASA
                if (IError == 0 && subtotalMovComercial!=subtotal)
                {                    
                    IError = Movimiento.fSetDatoMovimiento("CNETO", subtotal.ToString());
                    SDK.MuestraError(IError, "CNETO");
                    Console.WriteLine("***SE MODIFICA SUBTOTAL {0} POR {1}", subtotalMovComercial,subtotal);
                }
                //NO HAY ERROR Y EL IVA DE COMERCIAL ES DISTINTO AL QUE SE PASA
                if (IError == 0 && ivaMovComercial != iva)
                {

                    IError = Movimiento.fSetDatoMovimiento("CIMPUESTO1", iva.ToString());
                    SDK.MuestraError(IError, "CIMPUESTO1");
                    Console.WriteLine("***SE MODIFICA IVA {0} POR {1}", ivaMovComercial, iva);
                }
                //NO HAY ERROR SE PROCEDE A MODIFICAR EL TOTAL DEL MOVIMIENTO
                if (IError == 0)
                {                   
                    IError = Movimiento.fSetDatoMovimiento("CTOTAL", mov.importe.ToString());
                    SDK.MuestraError(IError, "CTOTAL");
                    Console.WriteLine("***SE MODIFICA TOTAL {0} POR {1}", totalMovComercial, mov.importe);
                }
                if (IError == 0)
                {
                    IError = Movimiento.fGuardaMovimiento();
                    SDK.MuestraError(IError, "fGuardaMovimiento");
                }
            }
                         
            SDK.MuestraError(IError, "crearMovimiento");
            return IError;
        }

        public static int crearMovimientoDescuento(int id_documento, EntityMovimiento mov)
        {


            int IError;
            int idMovimiento = 0;
            double totalMovComercial = 0;// se almacena el total del movimiento que este en el comercial
            double SubtotalMovComercial = 0;// se almacena el iva del movimiento de comercial
            double ivaMovComercial = 0;// se almacena el iva del movimiento de comercial
            double descuentoMovComercial = 0;// se almacena el iva del movimiento de comercial

            //se trunca a 3 decimales y se redonde a 2
            //nota el por mil en el truncate es para 3 y entre mil para volver a poner el decimal
            double subtotal = getSubtotal(mov.importe);//obtiene el subtotal
            double iva = getIva(mov.importe-mov.descuento);
            
            double subtotal_descuento = getSubtotal(mov.importe)- getSubtotal(mov.importe- mov.descuento);
            Console.WriteLine("***SUBTOTAL:{0}", subtotal);
            Console.WriteLine("***IVA:{0}", iva);
            Console.WriteLine("***DESCUENTO:{0}", subtotal_descuento);


            Movimiento.tMovimientoDesc movimiento_desc = new Movimiento.tMovimientoDesc();
            movimiento_desc.aCodAlmacen = "1";
            movimiento_desc.aConsecutivo = 1;
            movimiento_desc.aCodProdSer = mov.codProducto;
            movimiento_desc.aUnidades = mov.unidades;
            movimiento_desc.aPrecio = subtotal;
            movimiento_desc.aImporteDescto1 = subtotal_descuento;
            //movimiento_desc.aImporteDescto1 = mov.descuento;

            IError = Movimiento.fAltaMovimientoCDesct(id_documento, ref idMovimiento, ref movimiento_desc);
            SDK.MuestraError(IError, "fAltaMovimientoCDesct");
            if (IError == 0)
            {
                Console.WriteLine("MOVIMIENTO CON DESCUENTO DADO DE ALTA CON ID:{0}", idMovimiento);
                StringBuilder total_movimiento = new StringBuilder(12);
                IError = Movimiento.fLeeDatoMovimiento("CTOTAL", total_movimiento, 30);
                SDK.MuestraError(IError, "CTOTAL");
                if (IError == 0) totalMovComercial = Double.Parse(total_movimiento.ToString());
                Console.WriteLine("***TOTAL DE MOVIMIENTO CON DESCUENTO EN COMERCIAL:{0}", total_movimiento);
            }

            if (IError == 0 && totalMovComercial != (mov.importe-mov.descuento)) {

                Console.WriteLine("***INICIA PROCESO DE AJUSTE EN EL MOVIMIENTO TOTAL:{0} Y TOTAL COMERCIAL:{1}", (mov.importe-mov.descuento), totalMovComercial);

                if (IError == 0)
                {
                    StringBuilder subtotal_movimiento = new StringBuilder(12);
                    IError = Movimiento.fLeeDatoMovimiento("CNETO", subtotal_movimiento, 10);
                    if (IError == 0) SubtotalMovComercial = Double.Parse(subtotal_movimiento.ToString());
                    SDK.MuestraError(IError, "CNETO");
                    Console.WriteLine("***SUBTOTAL DE MOVIMIENTO CON DESCUENTO EN COMERCIAL:{0}", subtotal_movimiento);
                }

                //se recupera el iva del comercial 
                if (IError == 0)
                {
                    StringBuilder iva_movimiento = new StringBuilder(12);
                    IError = Movimiento.fLeeDatoMovimiento("CIMPUESTO1", iva_movimiento, 10);
                    if (IError == 0) ivaMovComercial = Double.Parse(iva_movimiento.ToString());
                    SDK.MuestraError(IError, "CIMPUESTO1");
                    Console.WriteLine("***IVA DE MOVIMIENTO CON DESCUENTO EN COMERCIAL:{0}", iva_movimiento);
                }
                //se recupera el subtotal del descuento en comercial
                if (IError == 0)
                {
                    StringBuilder descuento_movimiento = new StringBuilder(12);
                    IError = Movimiento.fLeeDatoMovimiento("CDESCUENTO1", descuento_movimiento, 10);
                    if (IError == 0) descuentoMovComercial = Double.Parse(descuento_movimiento.ToString());
                    SDK.MuestraError(IError, "CDESCUENTO1");
                    Console.WriteLine("***DESCUENTO DE MOVIMIENTO CON DESCUENTO EN COMERCIAL:{0}", descuento_movimiento);
                }
                

                if (IError == 0)
                {
                    Console.WriteLine("***INICIA EDICION DEL MOVIMIENTO");
                    IError = Movimiento.fEditarMovimiento();
                    SDK.MuestraError(IError, "fEditarMovimiento");
                }
                if (IError == 0)
                {
                    IError = Movimiento.fSetDatoMovimiento("CNETO", subtotal.ToString());
                    SDK.MuestraError(IError, "CNETO");
                    Console.WriteLine("***SE MODIFICA SUBTOTAL {0} POR {1}", SubtotalMovComercial, subtotal);
                }
                //NO HAY ERROR Y EL IVA DE COMERCIAL ES DISTINTO AL QUE SE PASA
                if (IError == 0 && ivaMovComercial != iva)
                {

                    IError = Movimiento.fSetDatoMovimiento("CIMPUESTO1", iva.ToString());
                    SDK.MuestraError(IError, "CIMPUESTO1");
                    Console.WriteLine("***SE MODIFICA IVA {0} POR {1}", ivaMovComercial, iva);
                }
                //NO HAY ERROR Y EL IVA DE COMERCIAL ES DISTINTO AL QUE SE PASA
                if (IError == 0 && descuentoMovComercial != subtotal_descuento)
                {

                    //IError = Movimiento.fSetDatoMovimiento("CDESCUENTO1", descuentoMovComercial.ToString());
                    IError = Movimiento.fSetDatoMovimiento("CDESCUENTO1", subtotal_descuento.ToString());
                    SDK.MuestraError(IError, "CDESCUENTO1");
                    Console.WriteLine("***SE MODIFICA SUBTOTAL DEL DESCUENTO {0} POR {1}", descuentoMovComercial, subtotal_descuento);
                }
                if (IError == 0)
                {
                    IError = Movimiento.fSetDatoMovimiento("CTOTAL", (mov.importe - mov.descuento).ToString());
                    SDK.MuestraError(IError, "CTOTAL");
                    Console.WriteLine("***SE MODIFICA TOTAL {0} POR {1}", totalMovComercial, (mov.importe - mov.descuento));
                }
                if (IError == 0)
                {
                    IError = Movimiento.fGuardaMovimiento();
                    SDK.MuestraError(IError, "fGuardaMovimiento");
                }
            }               

            SDK.MuestraError(IError, "crearMovimientoDescuento");
            return IError;

        }


        public static double getSubtotal(double total)
        {
            return Math.Round(Math.Truncate((total / 1.16) * 1000) / 1000, 2);
        }

        public static double getIva(double total)
        {
            //var sub_total =Math.Truncate((total / 1.16) * 1000) / 1000;
            var sub_total = Math.Round(Math.Truncate((total / 1.16) * 1000) / 1000, 2);
            //return Math.Round(Math.Truncate((sub_total * .16) * 1000) / 1000,2,MidpointRounding.AwayFromZero);
            return total - sub_total;
        }
    
    }
}
