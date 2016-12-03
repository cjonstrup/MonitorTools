using System.Collections.Generic;

namespace ServiceAgent.Routes
{
    public class ServiceDescription
    {
        public string Name { get; set; }

        public List<ServiceOperation> Howto { get; set; }
    }
}