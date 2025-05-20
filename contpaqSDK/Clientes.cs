using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace contpaqSDK
{
    public class Clientes
    {

        // declaracion de la estructura de cliente Provedor
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tCteProv
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string cCodigoCliente;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongNombre)]
            public string cRazonSocial;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongFecha)]
            public string cFechaAlta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongRFC)]
            public string cRFC;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCURP)]
            public string cCURP;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongDenComercial)]
            public string cDenComercial;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongRepLegal)]
            public string cRepLegal;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongNombre)]
            public string cNombreMoneda;
            public int cListaPreciosCliente;
            public double cDescuentoMovto;
            public int cBanVentaCredito;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionCliente1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionCliente2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionCliente3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionCliente4;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionCliente5;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionCliente6;
            public int cTipoCliente;
            public int cEstatus;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongFecha)]
            public string cFechaBaja;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongFecha)]
            public string cFechaUltimaRevision;
            public double cLimiteCreditoCliente;
            public int cDiasCreditoCliente;
            public int cBanExcederCredito;
            public double cDescuentoProntoPago;
            public int cDiasProntoPago;
            public double cInteresMoratorio;
            public int cDiaPago;
            public int cDiasRevision;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongDesCorta)]
            public string cMensajeria;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongDescripcion)]
            public string cCuentaMensajeria;
            public int cDiasEmbarqueCliente;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string cCodigoAlmacen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string cCodigoAgenteVenta;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string cCodigoAgenteCobro;
            public int cRestriccionAgente;
            public double cImpuesto1;
            public double cImpuesto2;
            public double cImpuesto3;
            public double cRetencionCliente1;
            public double cRetencionCliente2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionProveedor1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionProveedor2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionProveedor3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionProveedor4;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionProveedor5;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacionProveedor6;
            public double cLimiteCreditoProveedor;
            public int cDiasCreditoProveedor;
            public int cTiempoEntrega;
            public int cDiasEmbarqueProveedor;
            public double cImpuestoProveedor1;
            public double cImpuestoProveedor2;
            public double cImpuestoProveedor3;
            public double cRetencionProveedor1;
            public double cRetencionProveedor2;
            public int cBanInteresMoratorio;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTextoExtra)]
            public string cTextoExtra1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTextoExtra)]
            public string cTextoExtra2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTextoExtra)]
            public string cTextoExtra3;
            public double cImporteExtra1;
            public double cImporteExtra2;
            public double cImporteExtra3;
            public double cImporteExtra4;
        }

        // Declaración de funciones


        //funciones de alto nivel para Cliente Proveedor

        [DllImport("MGWServicios.dll")]
        public static extern int fAltaCteProv(ref int aIdCliente, ref tCteProv astCliente);


        [DllImport("MGWServicios.dll", EntryPoint = "fActualizaCteProv")]
        public static extern int fActualizaCteProv(string aCodigoCteProv, ref tCteProv astCteProv);

        //funciones de bajo nivel para Cliente Proveedor

        [DllImport("MGWServicios.DLL")]
        public static extern int fBuscaCteProv(String aCodCteProv);

        [DllImport("MGWServicios.DLL")]
        public static extern int fBuscaIdCteProv(int aIdCteProv);
        

        [DllImport("MGWServicios.DLL")]
        public static extern int fEditaCteProv();

        [DllImport("MGWServicios.DLL")]
        public static extern int fSetDatoCteProv(String aCampo, String aValor);

        [DllImport("MGWServicios.DLL")]
        public static extern int fLeeDatoCteProv(string aCampo, StringBuilder aValor, int aLen);

        [DllImport("MGWServicios.DLL")]
        public static extern int fLlenaRegistroCteProv(ref tCteProv astCtePro, int aEsAlta);

        [DllImport("MGWServicios.DLL")]
        public static extern int fGuardaCteProv();

        [DllImport("MGWServicios.DLL")]
        public static extern int fBorraCteProv(String aCodCteProv);
       

    }//Fin clase declaraciones
}
