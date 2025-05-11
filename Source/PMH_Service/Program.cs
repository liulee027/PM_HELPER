using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace PMH_Service
{
    internal class Program
    {
        static void Main(string[] args)
        {
#if DEBUG
            try
            {
                Service_Application service_Application = new Service_Application();
                service_Application.Start();

                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Main\r"+ex.ToString());
            }
           
#else
    ServiceBase.Run(new MyService());  
#endif

        }
    }
}
