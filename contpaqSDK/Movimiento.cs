using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace contpaqSDK
{
   public class Movimiento
    {
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tMovimiento
        {
            public int aConsecutivo;
            public double aUnidades;
            public double aPrecio;
            public double aCosto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodProdSer;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodAlmacen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongReferencia)]
            public string aReferencia;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodClasificacion;
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tMovimientoDesc
        {
            public int aConsecutivo;
            public double aUnidades;
            public double aPrecio;
            public double aCosto;
            public double aPorcDescto1;
            public double aImporteDescto1;
            public double aPorcDescto2;
            public double aImporteDescto2;
            public double aPorcDescto3;
            public double aImporteDescto3;
            public double aPorcDescto4;
            public double aImporteDescto4;
            public double aPorcDescto5;
            public double aImporteDescto5;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodProdSer;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodAlmacen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongReferencia)]
            public string aReferencia;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodClasificacion;
        }

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaMovimiento")]
        public static extern int fAltaMovimiento(int aIdDocumento, ref int aIdMovimiento, ref tMovimiento astMovimiento);

        /**[DllImport("MGWServicios.dll", EntryPoint = "fAltaMovimientoCaracteristicas")]
        public static extern int fAltaMovimientoCaracteristicas(int aIdMovimiento, int aIdMovtoCaracteristicas, tCaracteristicas aCaracteristicas);*/

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaMovimientoCaracteristicas_Param")]
        public static extern int fAltaMovimientoCaracteristicas_Param(string aIdMovimiento, string aIdMovtoCaracteristicas, string aUnidades, string aValorCaracteristica1, string aValorCaracteristica2, string aValorCaracteristica3);

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaMovimientoCDesct")]
        public static extern int fAltaMovimientoCDesct(int aIdDocumento, ref int aIdMovimiento, ref tMovimientoDesc astMovimiento);

        /**[DllImport("MGWServicios.dll", EntryPoint = "fAltaMovimientoEx")]
        public static extern int fAltaMovimientoEx(ref int aIdMovimiento, tTipoProducto aTipoProducto);

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaMovimientoSeriesCapas")]
        public static extern int fAltaMovimientoSeriesCapas(int aIdMovimiento, ref tSeriesCapas aSeriesCapas);*/

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaMovimientoSeriesCapas_Param")]
        public static extern int fAltaMovimientoSeriesCapas_Param(string aIdMovimiento, string aUnidades, string aTipoCambio, string aSeries, string aPedimento, string aAgencia, string aFechaPedimento, string aNumeroLote, string aFechaFabricacion, string aFechaCaducidad);

        [DllImport("MGWServicios.dll", EntryPoint = "fBorraMovimiento")]
        public static extern int fBorraMovimiento(int aIdDocumento, int aIdMovimiento);

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscarIdMovimiento")]
        public static extern int fBuscarIdMovimiento(int aIdMovimiento);

        [DllImport("MGWServicios.dll", EntryPoint = "fCancelaCambiosMovimiento")]
        public static extern int fCancelaCambiosMovimiento();

        [DllImport("MGWServicios.dll", EntryPoint = "fCancelaFiltroMovimiento")]
        public static extern int fCancelaFiltroMovimiento();

        [DllImport("MGWServicios.dll", EntryPoint = "fEditarMovimiento")]
        public static extern int fEditarMovimiento();

        [DllImport("MGWServicios.dll", EntryPoint = "fGuardaMovimiento")]
        public static extern int fGuardaMovimiento();

        [DllImport("MGWServicios.dll", EntryPoint = "fInsertarMovimiento")]
        public static extern int fInsertarMovimiento();

        [DllImport("MGWServicios.dll", EntryPoint = "fLeeDatoMovimiento")]
        public static extern int fLeeDatoMovimiento(string aCampo, StringBuilder aValor, int aLen);

        [DllImport("MGWServicios.dll", EntryPoint = "fPosAnteriorMovimiento")]
        public static extern int fPosAnteriorMovimiento();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosMovimientoBOF")]
        public static extern int fPosMovimientoBOF();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosMovimientoEOF")]
        public static extern int fPosMovimientoEOF();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosPrimerMovimiento")]
        public static extern int fPosPrimerMovimiento();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosSiguienteMovimiento")]
        public static extern int fPosSiguienteMovimiento();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosUltimoMovimiento")]
        public static extern int fPosUltimoMovimiento();

        [DllImport("MGWServicios.dll", EntryPoint = "fSetDatoMovimiento")]
        public static extern int fSetDatoMovimiento(string aCampo, string aValor);

        [DllImport("MGWServicios.dll", EntryPoint = "fSetFiltroMovimiento")]
        public static extern int fSetFiltroMovimiento(int aIdDocumento);

        [DllImport("MGWServicios.dll", EntryPoint = "fModificaCostoEntrada")]
        public static extern int fModificaCostoEntrada(string aIdMovimiento, string aCostoEntrada);

    }
}
