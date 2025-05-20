using comercial;
using restApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace host
{
    class Program
    {
        static void Main(string[] args)
        {

           
            try
             {
                 //obtiene la baseurl desde el App.config
                 var epAddress = System.Configuration.ConfigurationManager.AppSettings["baseAddress"];
                //crea uri con la base url
                 Uri baseAddress = new Uri(epAddress);

                //usa el cors y el wcf de restApi proyecto
                 using (var serviceHost = new CorsEnabledServiceHost(typeof(WCFCompaq), baseAddress))
                 {
                    //si es la url contiene https
                     if (epAddress.Contains("https:"))
                     {
                         try
                         {
                             Console.WriteLine(System.Configuration.ConfigurationManager.AppSettings["baseAddress"]);
                             // Check to see if the service host already has a ServiceMetadataBehavior
                             ServiceMetadataBehavior smb = serviceHost.Description.Behaviors.Find<ServiceMetadataBehavior>();
                             bool add = false;

                             // If not, add one
                             if (smb == null)
                             {
                                 Console.WriteLine("ServiceMetadataBehavior");
                                 smb = new ServiceMetadataBehavior();
                                 add = true;
                             }

                             Console.WriteLine("HttpsGetEnabled");
                             smb.HttpsGetEnabled = true;
                             //smb.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;

                             if (add)
                             {
                                 Console.WriteLine("Add ServiceMetadataBehavior");
                                 serviceHost.Description.Behaviors.Add(smb);
                             }
                         }
                         catch (Exception ex)
                         {
                             System.Console.WriteLine(ex.Message);
                             System.Console.WriteLine(ex.StackTrace);
                             System.Console.WriteLine("Press enter to quit ");
                             System.Console.ReadLine();
                         }
                     }

                     Console.WriteLine("SERVIDOR:" + epAddress.ToString());
                     ComercialMain.abrirEmpresa();//INICIA COMERCIAL
                     serviceHost.Open();
                     System.Console.WriteLine("SERVIDOR ESTA CORRIENDO");
                     System.Console.WriteLine("PRESIONAR ENTER PARA SALIR...");
                     System.Console.ReadLine();
                     serviceHost.Close();
                     ComercialMain.cerrarEmpresa();//CIERRA COMERCIAL
                }
             }
             catch (Exception ex2)
             {
                 System.Console.WriteLine(ex2.Message);
                 System.Console.WriteLine(ex2.StackTrace);
                 System.Console.WriteLine("Press enter to quit ");
                 System.Console.ReadLine();
                 throw new FaultException(ex2.Message);
             }
        }
    }
}
