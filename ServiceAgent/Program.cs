using System.Configuration;
using Topshelf;

namespace ServiceAgent
{
    class Program
    {
        static void Main(string[] args)
        {

            HostFactory.Run(x =>
            {
                //x.UseLinuxIfAvailable();

                var port = ConfigurationManager.AppSettings["PORT"] ?? "5000";

                x.Service<NancySelfHost>(s =>
                {
                    s.ConstructUsing(name => new NancySelfHost());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Custom Monitor Agent");
                x.SetDisplayName("Monitor Agent");
                x.SetServiceName("MonitorAgent");

                x.AfterInstall(() =>
                {
                    Tools.ExecuteCommandSync("netsh advfirewall firewall add rule name=\"CJ Service Agent\" dir=in action=allow protocol=TCP localport=" + port);
                });

                x.AfterUninstall(() =>
                {
                    Tools.ExecuteCommandSync("netsh advfirewall firewall delete rule name=\"CJ Service Agent\" dir=in protocol=TCP localport=" + port);
                });

                x.EnableServiceRecovery(r =>
                {
                    //you can have up to three of these
                    r.RestartService(0);
                    r.RestartService(1);
                    //the last one will act for all subsequent failures
                    //r.RunProgram(7, "ping google.com");

                    //should this be true for crashed or non-zero exits
                    r.OnCrashOnly();

                    //number of days until the error count resets
                    r.SetResetPeriod(1);
                });

                x.RunAsLocalSystem();
                x.Disabled();



            });


        }
    }
}
