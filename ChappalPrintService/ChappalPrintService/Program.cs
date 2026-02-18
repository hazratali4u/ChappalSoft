using System;
using System.Collections.Generic;
using System.ServiceProcess;

namespace ChappalPrintService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun = new ServiceBase[]
            {
                new InvoicePrint()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
