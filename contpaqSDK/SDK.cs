using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace contpaqSDK
{
    public class SDK
    {
        #region funciones generales  

        [DllImport("KERNEL32")]
        public static extern int SetCurrentDirectory(string pPtrDirActual);

        [DllImport("MGWServicios.dll")]
        public static extern int fInicioSesionSDK(string usuario,string password);

        [DllImport("MGWServicios.dll", EntryPoint = "fInicioSesionSDKCONTPAQi")]
        public static extern void fInicioSesionSDKCONTPAQi(string aUsuario, string aContrasenia);

        [DllImport("MGWServicios.dll")]
        public static extern int fSetNombrePAQ(string aNombrePAQ);

        [DllImport("MGWServicios.dll")]
        public static extern int fAbreEmpresa(string aDirEmpresa);

        [DllImport("MGWServicios.dll")]
        public static extern int fCierraEmpresa();

        [DllImport("MGWServicios.dll")]
        public static extern int fTerminaSDK();


        [DllImport("MGWServicios.dll")]
        public static extern int fError(int NumeroError, StringBuilder Mensaje, int Longitud);

        #endregion


        #region Método Manejo de Errores
        public static string MuestraError(int NumeroError, string funcion)
        {
            StringBuilder MensajeError = new StringBuilder(512);
            const int tamaño = 512;

            if (NumeroError != 0)
            {
                SDK.fError(NumeroError, MensajeError, tamaño);
                Console.WriteLine("Error: " + MensajeError.ToString() + " En : " + funcion);
                return "Error: " + MensajeError.ToString() + " En : " + funcion;
            }
            return "sin Error";
        }//Fin Método Error 

        public static string MuestraErrorLogs(int NumeroError, string funcion)
        {
            StringBuilder MensajeError = new StringBuilder(512);
            const int tamaño = 512;

            if (NumeroError != 0)
            {
                SDK.fError(NumeroError, MensajeError, tamaño);               
                return "Error: " + MensajeError.ToString() + " En : " + funcion;
            }
            else return "SIN ERROR CONTINUA...";
        }//Fin Método Error 
        #endregion
    }
}
