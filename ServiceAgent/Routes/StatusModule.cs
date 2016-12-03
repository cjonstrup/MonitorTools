using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using Nancy;

namespace ServiceAgent.Routes
{
    public class StatusModule : NancyModule
    {
        public StatusModule()
        {
            // this.RequiresAuthentication();
            //  this.RequiresClaims(new[] { "Admin" });

            Get["/"] = parameters => Response.AsText("Service running");

            Get["/v1/status"] = parameters =>
            {
               var _services = ConfigurationManager.AppSettings["SERVICES"].ToLower().Split(',').ToList();

                var finish = new List<ServiceInfo>();

                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    if (_services.Contains(service.ServiceName.ToLower()))
                    {
                        finish.Add(new ServiceInfo() { Name = service.ServiceName.ToLower(), Status = service.Status.ToString(), StatusId = service.Status });
                    }
                }

                return Response.AsJson(new StatusResponse()
                {
                    Services = finish,
                    Description = new ServiceDescription()
                    {
                        Name = "Shows list of monitored services.",
                        Howto = new List<ServiceOperation>()
                        {
                            new ServiceOperation() {Name = "Add",Description = "/v1/admin/service/add/{service}"},
                            new ServiceOperation() {Name = "Remove",Description = "/v1/admin/service/remove/{service}"},
                            new ServiceOperation() {Name = "List",Description = "/v1/admin/services"}
                        }
                    }
                });
            };
        }
    }
}
