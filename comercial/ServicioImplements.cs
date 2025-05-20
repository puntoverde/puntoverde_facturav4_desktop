using entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using contpaqSDK;

namespace comercial
{
    public class ServicioImplements
    {
        /*
        public static int createOrUpdateServicio(List<EntentyMovimiento> movimientos, Pago pago) {
            int IError=0;//variable para los errores de contpaq

            foreach (EntentyMovimiento movimiento in movimientos)
            {

                int IErrorProducto = Producto.fBuscaProducto(movimiento.codigoProducto);
                if (IErrorProducto==0)
                {
                    StringBuilder IdServicio= new StringBuilder(12);
                    Producto.fLeeDatoProducto("CIDPRODUCTO", IdServicio,12);
                    Console.WriteLine("IDPRODUCTO:{0}",IdServicio.ToString());
                    IError = IError + updateServicio(int.Parse(IdServicio.ToString()), movimiento, pago);
                }
                else {
                
                IError = IError + createServicio(movimiento, pago);
                }

            }
            
            SDK.MuestraError(IError, "createOrUpdateServicio");
            return IError;
        }

        public static int createServicio(EntentyMovimiento movimiento,Pago pago)
        {
            int IError;//variable para los errores de contpaq
            int IdServicio = 0;//variable del id de producto o servicio

            //signacion de los parametros del servicio
            Producto.tProducto servicio = new Producto.tProducto();
            servicio.cCodigoProducto = movimiento.codigoProducto;
            servicio.cNombreProducto = movimiento.descripcion;
            servicio.cTipoProducto = 3; // SERVICIO.
            servicio.cCodigoUnidadBase = "SERVICIO";
            servicio.cControlExistencia = 1;
            servicio.cImpuesto1 = 0; //se establece el iva en 0
            servicio.cTextoExtra2 = pago.MesPago;
            //servicio.cDescripcionProducto = "dec";
            //if (m.IVA > 0)
            //{
              //  producto.cImpuesto1 = 16;
            //}


            IError = Producto.fAltaProducto(ref IdServicio, ref servicio);
            SDK.MuestraError(IError, "fAltaProducto");
            if (IError == 0)//en cero significa que el cliente se dio de alta correctamente...
            {
                Console.WriteLine("servico ${0} creado:", IdServicio);

                String descripcionServicio = $@" Venta Concepto:{movimiento.descripcion} Alumno:{pago.Matricula} {pago.NombreAlumno}";

                //se grega el complemento educativo para las colegiaturas
                if (pago.isColegiatura) 
                {
                IError = Producto.fInsertaDatoCompEducativo(IdServicio, 1, pago.NombreAlumno);//nombre alumno
                    SDK.MuestraError(IError, "fInsertaDatoCompEducativo(NombreAlumno)");
                IError = Producto.fInsertaDatoCompEducativo(IdServicio, 2, pago.CURP);//curp
                    SDK.MuestraError(IError, "fInsertaDatoCompEducativo(CURP)");
                IError = Producto.fInsertaDatoCompEducativo(IdServicio, 3, pago.NivelEducativo);//nivel educativo
                    SDK.MuestraError(IError, "fInsertaDatoCompEducativo(NivelEducativo)");
                IError = Producto.fInsertaDatoCompEducativo(IdServicio, 4, pago.ClaveRVOE);//rvoe
                    SDK.MuestraError(IError, "fInsertaDatoCompEducativo(ClaveRVOE)");
                IError = Producto.fInsertaDatoCompEducativo(IdServicio, 5, pago.Rfc);//rfc
                    SDK.MuestraError(IError, "fInsertaDatoCompEducativo(RFC)");


                    descripcionServicio = $@" CLAVE SAT:{movimiento.claveSat} Concepto: {movimiento.descripcion} Alumno:{pago.NombreAlumno} CURP:{pago.CURP} Seccion:{pago.NivelEducativo} AutRVOE:{pago.ClaveRVOE}";
                }

                //habilita la modificacion del servicio
                IError = Producto.fEditaProducto();
                SDK.MuestraError(IError, "fEditaProducto");
                if (IError == 0)
                {
                    Console.WriteLine("EDICION DEL SERVICIO: {0} HABILITADA", IdServicio);                    
                   
                    //IError = Producto.fSetDatoProducto("CNOMBREP01", movimiento.descripcion);
                    //SDK.MuestraError(IError, "CNOMBREP01");

                    //IError = Producto.fSetDatoProducto("CDESCRIP01", descripcionServicio);
                    //SDK.MuestraError(IError, "CDESCRIP01");
                    IError = Producto.fSetDatoProducto("CDESCRIPCIONPRODUCTO", descripcionServicio);
                    SDK.MuestraError(IError, "CDESCRIPCIONPRODUCTO");

                    IError = Producto.fSetDatoProducto("CCLAVESAT", movimiento.claveSat);
                    SDK.MuestraError(IError, "CCLAVESAT");

                    //IError = Producto.fSetDatoProducto("CESEXENTO", "1");//excento de iva
                    //SDK.MuestraError(IError, "CESEXENTO");

                    IError = Producto.fGuardaProducto();
                    SDK.MuestraError(IError, "fGuardaProducto");
                }
            }
            SDK.MuestraError(IError, "createServicio");
            //regresa el resultado si es 0 todo bien cualquier otro se puede ver que fue con MuestraError del sdk
            return IError;

        }

        public static int updateServicio(int IdServicio, EntentyMovimiento movimiento,Pago pago)
        {
            int IError;//variable para mostrar los errorer
          
            //signacion de los parametros del servicio
            Producto.tProducto servicio = new Producto.tProducto();            
            servicio.cNombreProducto = movimiento.descripcion;
            servicio.cTipoProducto = 3; // SERVICIO.
            servicio.cCodigoUnidadBase = "SERVICIO";
            servicio.cControlExistencia = 1;
            servicio.cDescripcionProducto = "";
            servicio.cImpuesto1 = 0; //se establece el iva en 0
            servicio.cTextoExtra2 = pago.MesPago;

            //se actualiza el cliente 
            IError = Producto.fActualizaProducto(movimiento.codigoProducto, ref servicio);
            SDK.MuestraError(IError, "fActualizaProducto");
            if (IError == 0)//si es 0 se actualizo correctamente
            {
                Console.WriteLine("SERVICIO: ${0} ACTUALIZADO", movimiento.codigoProducto);


                String descripcionServicio = $@" Venta Concepto:{movimiento.descripcion} Alumno:{pago.Matricula} {pago.NombreAlumno}";

                //se grega el complemento educativo para las colegiaturas
                if (pago.isColegiatura)
                {
                    IError = Producto.fInsertaDatoCompEducativo(IdServicio, 1, pago.NombreAlumno);//nombre alumno
                        SDK.MuestraError(IError, "fInsertaDatoCompEducativo(NombreAlumno)");
                    IError = Producto.fInsertaDatoCompEducativo(IdServicio, 2, pago.CURP);//curp
                        SDK.MuestraError(IError, "fInsertaDatoCompEducativo(CURP)");
                    IError = Producto.fInsertaDatoCompEducativo(IdServicio, 3, pago.NivelEducativo);//nivel educativo
                        SDK.MuestraError(IError, "fInsertaDatoCompEducativo(NivelEducativo)");
                    IError = Producto.fInsertaDatoCompEducativo(IdServicio, 4, pago.ClaveRVOE);//rvoe
                        SDK.MuestraError(IError, "fInsertaDatoCompEducativo(ClaveRVOE)");
                    IError = Producto.fInsertaDatoCompEducativo(IdServicio, 5, pago.Rfc);//rfc
                        SDK.MuestraError(IError, "fInsertaDatoCompEducativo(RFC)");

                    descripcionServicio = $@" CLAVE SAT:{movimiento.claveSat} Concepto: {movimiento.descripcion} Alumno:{pago.NombreAlumno} CURP:{pago.CURP} Seccion:{pago.NivelEducativo} AutRVOE:{pago.ClaveRVOE}";
                }

                //habilita la modificacion del cliente
                //habilita la modificacion del servicio
                IError = Producto.fEditaProducto();
                SDK.MuestraError(IError, "fEditaProducto");
                if (IError == 0)
                {
                    Console.WriteLine("EDICION DEL SERVICIO: {0} HABILITADA", movimiento.codigoProducto);

                    //IError = Producto.fSetDatoProducto("CNOMBREP01", movimiento.descripcion);
                    //SDK.MuestraError(IError, "CNOMBREP01");

                    //IError = Producto.fSetDatoProducto("CDESCRIP01", descripcionServicio);
                    //SDK.MuestraError(IError, "CDESCRIP01");

                    IError = Producto.fSetDatoProducto("CDESCRIPCIONPRODUCTO", descripcionServicio);
                    SDK.MuestraError(IError, "CDESCRIPCIONPRODUCTO");

                    IError = Producto.fSetDatoProducto("CCLAVESAT", movimiento.claveSat);
                    SDK.MuestraError(IError, "CCLAVESAT");

                    IError = Producto.fSetDatoProducto("CESEXENTO", "1");//excento de iva,(no aplica el iva)
                    SDK.MuestraError(IError, "CESEXENTO");

                    IError = Producto.fGuardaProducto();
                    SDK.MuestraError(IError, "fGuardaProducto");
                }
            }
            Console.WriteLine(SDK.MuestraErrorLogs(IError, "updateCliente"));

            //regresa el resultado si es 0 todo bien cualquier otro se puede ver que fue con MuestraError del sdk
            return IError;
        }
        */
    }
}
