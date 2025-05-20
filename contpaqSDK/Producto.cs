using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace contpaqSDK
{
    public class Producto
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tProducto
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string cCodigoProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongNombre)]
            public string cNombreProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongNombreProducto)]
            public string cDescripcionProducto;
            public int cTipoProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongFecha)]
            public string cFechaAltaProducto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongFecha)]
            public string cFechaBaja;
            public int cStatusProducto;
            public int cControlExistencia;
            public int cMetodoCosteo;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string cCodigoUnidadBase;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string cCodigoUnidadNoConvertible;
            public double cPrecio1;
            public double cPrecio2;
            public double cPrecio3;
            public double cPrecio4;
            public double cPrecio5;
            public double cPrecio6;
            public double cPrecio7;
            public double cPrecio8;
            public double cPrecio9;
            public double cPrecio10;
            public double cImpuesto1;
            public double cImpuesto2;
            public double cImpuesto3;
            public double cRetencion1;
            public double cRetencion2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongNombre)]
            public string cNombreCaracteristica1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongNombre)]
            public string cNombreCaracteristica2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongNombre)]
            public string cNombreCaracteristica3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion4;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion5;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodValorClasif)]
            public string cCodigoValorClasificacion6;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTextoExtra)]
            public string cTextoExtra1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTextoExtra)]
            public string cTextoExtra2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTextoExtra)]
            public string cTextoExtra3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongFecha)]
            public string cFechaExtra;
            public double cImporteExtra1;
            public double cImporteExtra2;
            public double cImporteExtra3;
            public double cImporteExtra4;
        }

        [DllImport("MGWServicios.dll", EntryPoint = "fPosPrimerProducto")]
        public static extern int fPosPrimerProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosSiguienteProducto")]
        public static extern int fPosSiguienteProducto();

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscaProducto")]
        public static extern int fBuscaProducto(string aCodProducto);

        [DllImport("MGWServicios.dll", EntryPoint = "fLeeDatoProducto")]
        public static extern int fLeeDatoProducto(string aCampo, StringBuilder aValor, int aLen);

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscaIdProducto")]
        public static extern int fBuscaIdProducto(int aIdProducto);

    }
}
