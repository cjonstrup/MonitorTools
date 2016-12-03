using System;
using System.Configuration;
using Nancy.Hosting.Self;

namespace ServiceAgent
{
    public class NancySelfHost
    {
        private NancyHost _mNancyHost;
        private string _url = "http://localhost";
        private string _port;


        public void Start()
        {
            try
            {
                HostConfiguration hostConfigs = new HostConfiguration()
                {
                    UrlReservations = new UrlReservations() { CreateAutomatically = true }
                };

                _port = ConfigurationManager.AppSettings["PORT"] ?? "5000";
                _mNancyHost = new NancyHost(new Uri(_url + ":" + _port));
                
                _mNancyHost.Start();

                Console.WriteLine(_url+ ":" + _port);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Stop()
        {
            _mNancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}