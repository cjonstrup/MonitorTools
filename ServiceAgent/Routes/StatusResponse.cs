using System.Collections.Generic;

namespace ServiceAgent.Routes
{
    public class StatusResponse
    {
        public StatusResponse()
        {
        }

        public List<ServiceInfo> Services { get; set; }

        public ServiceDescription Description { get; set; }
    }
}