using System.ServiceProcess;

namespace ServiceAgent.Routes
{
    public class ServiceInfo
    {
        public string Name { get; set; }
        public string Status { get; set; }
        public ServiceControllerStatus StatusId { get; set; }
    }
}