using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;

namespace MvcWebRole
{
    public class WebRole : RoleEntryPoint
    {
        private void ConfigureDiagnostics()
        {
            DiagnosticMonitorConfiguration config = DiagnosticMonitor.GetDefaultInitialConfiguration();
            config.Logs.BufferQuotaInMB = 500;
            config.Logs.ScheduledTransferLogLevelFilter = LogLevel.Verbose;
            config.Logs.ScheduledTransferPeriod = TimeSpan.FromMinutes(1d);

            DiagnosticMonitor.Start(
                "Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString",
                config);
        }


        public override bool OnStart()
        {
            ConfigureDiagnostics();
            // For information on handling configuration changes
            // see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

            return base.OnStart();
        }

        public override void OnStop()
        {
            Trace.TraceInformation("OnStop called from WebRole");
            var counter = new PerformanceCounter("ASP.NET", "Request Current", "");
            while (counter.NextValue()>0)
            {
                Trace.TraceInformation("ASP.NET Request Current: " + counter.NextValue().ToString());
                System.Threading.Thread.Sleep(1000);
            }
            base.OnStop();
        }
    }
}
