
using PMH_Service;
using System;
using System.IO;
using System.IO.Pipes;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

public class MyService : ServiceBase
{

   

    protected override void OnStart(string[] args)
    {
        Service_Application service_Application = new Service_Application();
        service_Application.Start();

    }

 

    protected override void OnStop()
    {
     

           
    }

 
}
