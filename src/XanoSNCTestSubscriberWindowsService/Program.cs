using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XanoSNCTestSubscriberWindowsService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
#if !DEBUG
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new XanoSNCTestSubscriberWindowsService()
            };
            ServiceBase.Run(ServicesToRun);
#else
            var service = new XanoSNCTestSubscriberWindowsService();
            service.DebugThisService();
#endif
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
