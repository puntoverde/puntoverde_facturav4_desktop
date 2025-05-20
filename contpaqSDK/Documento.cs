using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace contpaqSDK
{
    public class Documento
    {

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tDocumento
        {
            public double aFolio;
            public int aNumMoneda;
            public double aTipoCambio;
            public double aImporte;
            public double aDescuentoDoc1;
            public double aDescuentoDoc2;
            public int aSistemaOrigen;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodConcepto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongSerie)]
            public string aSerie;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongFecha)]
            public string aFecha;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodigoCteProv;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodigoAgente;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongReferencia)]
            public string aReferencia;
            public int aAfecta;
            public double aGasto1;
            public double aGasto2;
            public double aGasto3;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 4)]
        public struct tLlaveDoc
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongCodigo)]
            public string aCodConcepto;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = Constantes.kLongSerie)]
            public string aSerie;
            public double aFolio;
        }


        [DllImport("MGWServicios.dll", EntryPoint = "fAltaDocumento")]
        public static extern int fAltaDocumento(ref int aIdDocumento, ref tDocumento aDocumento);

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaDocumentoCargoAbono")]
        public static extern int fAltaDocumentoCargoAbono(ref tDocumento aDocumento);

        [DllImport("MGWServicios.dll", EntryPoint = "fAltaDocumentoCargoAbonoExtras")]
        public static extern int fAltaDocumentoCargoAbonoExtras(ref tDocumento aDocumento, string aTextoExtra1, string aTextoExtra2, string aTextoExtra3, string aFechaExtra, double aImporteExtra1, double aImporteExtra2, double aImporteExtra3, double aImporteExtra4);


        [DllImport("MGWServicios.dll", EntryPoint = "fSiguienteFolio")]
        public static extern int fSiguienteFolio(string aCodigoConcepto, StringBuilder aSerie, ref double aFolio);


        [DllImport("MGWServicios.dll", EntryPoint = "fBorraDocumento")]
        public static extern int fBorraDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fBorraDocumento_CW")]
        public static extern int fBorraDocumento_CW();

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscaDocumento")]
        public static extern int fBuscaDocumento(ref tLlaveDoc aLlaveDocto);

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscarDocumento")]
        public static extern int fBuscarDocumento(string aCodConcepto, string aSerie, string aFolio);

        [DllImport("MGWServicios.dll", EntryPoint = "fBuscarIdDocumento")]
        public static extern int fBuscarIdDocumento(int aIdDocumento);

        [DllImport("MGWServicios.dll", EntryPoint = "fCancelaDocumento")]
        public static extern int fCancelaDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fCancelaDocumento_CW")]
        public static extern int fCancelaDocumento_CW();

        [DllImport("MGWServicios.dll", EntryPoint = "fCancelaFiltroDocumento")]
        public static extern int fCancelaFiltroDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fCancelarModificacionDocumento")]
        public static extern int fCancelarModificacionDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fDesbloqueaDocumento")]
        public static extern int fDesbloqueaDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fDocumentoBloqueado")]
        public static extern int fDocumentoBloqueado(ref int aImpreso);

        [DllImport("MGWServicios.dll", EntryPoint = "fDocumentoDevuelto")]
        public static extern int fDocumentoDevuelto(ref int aDevuelto);

        [DllImport("MGWServicios.dll", EntryPoint = "fDocumentoImpreso")]
        public static extern int fDocumentoImpreso(ref bool aImpreso);

        [DllImport("MGWServicios.dll", EntryPoint = "fDocumentoUUID")]
        //public static extern int fDocumentoUUID(StringBuilder aCodConcepto, StringBuilder aSerie, double aFolio,StringBuilder atPtrCFDIUUID);
        public static extern int fDocumentoUUID(StringBuilder aCodConcepto, StringBuilder aSerie, double aFolio, StringBuilder atPtrCFDIUUID);


        [DllImport("MGWServicios.dll", EntryPoint = "fEditarDocumento")]
        public static extern int fEditarDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fEditarDocumentoCheqpaq")]
        public static extern int fEditarDocumentoCheqpaq();

        [DllImport("MGWServicios.dll", EntryPoint = "fEmitirDocumento")]
        public static extern int fEmitirDocumento([MarshalAs(UnmanagedType.LPStr)] string aCodConcepto, [MarshalAs(UnmanagedType.LPStr)] string aSerie, double aFolio, [MarshalAs(UnmanagedType.LPStr)] string aPassword, [MarshalAs(UnmanagedType.LPStr)] string aArchivoAdicional);

        [DllImport("MGWServicios.dll", EntryPoint = "fEntregEnDiscoXML")]
        public static extern int fEntregEnDiscoXML(string aCodConcepto, string aSerie, double aFolio, int aFormato, string aFormatoAmig);

        [DllImport("MGWServicios.dll", EntryPoint = "fGuardaDocumento")]
        public static extern int fGuardaDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fInsertarDocumento")]
        public static extern int fInsertarDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fLeeDatoDocumento")]
        public static extern int fLeeDatoDocumento(string aCampo, StringBuilder aValor, int aLongitud);

        [DllImport("MGWServicios.dll", EntryPoint = "fPosAnteriorDocumento")]
        public static extern int fPosAnteriorDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosPrimerDocumento")]
        public static extern int fPosPrimerDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosSiguienteDocumento")]
        public static extern int fPosSiguienteDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fPosUltimoDocumento")]
        public static extern int fPosUltimoDocumento();

        [DllImport("MGWServicios.dll", EntryPoint = "fSaldarDocumento")]
        public static extern int fSaldarDocumento(ref tLlaveDoc aDoctoaPagar,ref  tLlaveDoc aDoctoPago, double aImporte, int aIdMoneda, string aFecha);
        //public static extern int fSaldarDocumento(ref RegLlaveDoc factura, ref RegLlaveDoc pago, double importe, int moneda, string fecha);

        [DllImport("MGWServicios.dll", EntryPoint = "fSaldarDocumento_Param")]
        public static extern int fSaldarDocumento_Param(string aCodConcepto_Pagar, string aSerie_Pagar, double aFolio_Pagar, string aCodConcepto_Pago, string aSerie_Pago, double aFolio_Pago, double aImporte, int aIdMoneda, string aFecha);

        [DllImport("MGWServicios.dll", EntryPoint = "fSaldarDocumentoCheqPAQ")]
        public static extern int fSaldarDocumentoCheqPAQ(tLlaveDoc aDoctoaPagar, tLlaveDoc aDoctoPago, double aImporte, int aIdMoneda, string aFecha, double aTipoCambioCheqPAQ);

        [DllImport("MGWServicios.dll", EntryPoint = "fSetDatoDocumento")]
        public static extern int fSetDatoDocumento(string aCampo, string aValor);

        [DllImport("MGWServicios.dll", EntryPoint = "fSetFiltroDocumento")]
        public static extern int fSetFiltroDocumento(string aFechaInicio, string aFechaFin, string aCodigoConcepto, string aCodigoCteProv);


        [DllImport("MGWServicios.dll", EntryPoint = "fAfectaDocto_Param")]
        public static extern int fAfectaDocto_Param(string aCodConcepto, string aSerie, double aFolio, bool aAfecta);



    }
}
