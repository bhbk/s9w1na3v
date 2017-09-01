using System.ServiceProcess;

namespace Bmon.Client.Svc
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new Daemon() 
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
