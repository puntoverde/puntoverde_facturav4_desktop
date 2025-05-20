using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace contpaqSDK
{
    public class Direccion
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tDireccion
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string cCodCteProv;
            public int cTipoCatalogo;
            public int cTipoDireccion;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongDescripcion)]
            public string cNombreCalle;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongNumeroExtInt)]
            public string cNumeroExterior;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongNumeroExtInt)]
            public string cNumeroInterior;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongDescripcion)]
            public string cColonia;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigoPostal)]
            public string cCodigoPostal;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTelefono)]
            public string cTelefono1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTelefono)]
            public string cTelefono2;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTelefono)]
            public string cTelefono3;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongTelefono)]
            public string cTelefono4;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongEmailWeb)]
            public string cEmail;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongEmailWeb)]
            public string cDireccionWeb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongDescripcion)]
            public string cCiudad;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongDescripcion)]
            public string cEstado;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongDescripcion)]
            public string cPais;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongDescripcion)]
            public string cTextoExtra;
        }


        //funciones de alto nivel

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaDireccion")]
        public static extern int fAltaDireccion(ref int aIdDireccion, ref tDireccion astDireccion);

        [DllImport("MGWServicios.dll", EntryPoint = "fActualizaDireccion")]
        public static extern int fActualizaDireccion(ref tDireccion astDireccion);

        [DllImport("MGWServicios.dll", EntryPoint = "fLlenaRegistroDireccion")]
        public static extern int fLlenaRegistroDireccion(ref tDireccion astDireccion, int aEsAlta);


        //funciones de bajo nivel
        [DllImport("MGWServicios.dll", EntryPoint = "fBuscaDireccionCteProv")]
        public static extern int fBuscaDireccionCteProv(string aCodCteProv, byte aTipoDireccion);

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscaDireccionDocumento")]
        public static extern int fBuscaDireccionDocumento(int aIdDocumento, byte aTipoDireccion);

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscaDireccionEmpresa")]
        public static extern int fBuscaDireccionEmpresa();

        [DllImport("MGWServicios.dll", EntryPoint = "fCancelarModificacionDireccion")]
        public static extern int fCancelarModificacionDireccion();

        [DllImport("MGWServicios.dll", EntryPoint = "fEditaDireccion")]
        public static extern int fEditaDireccion();

        [DllImport("MGWServicios.dll", EntryPoint = "fGuardaDireccion")]
        public static extern int fGuardaDireccion();

        [DllImport("MGWServicios.dll", EntryPoint = "fInsertaDireccion")]
        public static extern int fInsertaDireccion();

        [DllImport("MGWServicios.dll", EntryPoint = "fLeeDatoDireccion")]
        public static extern int fLeeDatoDireccion(string aCampo, StringBuilder aValor, int aLen);

        

        [DllImport("MGWServicios.dll", EntryPoint = "fPosAnteriorDireccion")]
        public static extern int fPosAnteriorDireccion();


        [DllImport("MGWServicios.dll", EntryPoint = "fPosBOFDireccion")]
        public static extern int fPosBOFDireccion();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosEOFDireccion")]
        public static extern int fPosEOFDireccion();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosPrimerDireccion")]
        public static extern int fPosPrimerDireccion();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosSiguienteDireccion")]
        public static extern int fPosSiguienteDireccion();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosUltimaDireccion")]
        public static extern int fPosUltimaDireccion();


        [DllImport("MGWServicios.dll", EntryPoint = "fSetDatoDireccion")]
        public static extern int fSetDatoDireccion(string aCampo, string aValor);

    }
}
