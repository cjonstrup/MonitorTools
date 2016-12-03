using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceProcess;
using Nancy;
using Nancy.Security;

namespace ServiceAgent.Routes
{
    public class AdminModule : NancyModule
    {
        public AdminModule()
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Admin" });

            Get["/v1/admin/services"] = parameters =>
            {
                //_services = ConfigurationManager.AppSettings["SERVICES"].Split(',').ToList();

                var finish = new List<ServiceInfo>();

                ServiceController[] services = ServiceController.GetServices();
                foreach (ServiceController service in services)
                {
                    finish.Add(new ServiceInfo() { Name = service.ServiceName.ToLower(), Status = service.Status.ToString(), StatusId = service.Status });
                }

                //return Response.AsJson(finish);
                return View["services.html", finish];
            };

            Get["/v1/admin/service/add/{name}"] = parameters =>
            {
                try
                {
                    var _services = ConfigurationManager.AppSettings["SERVICES"].ToLower().Split(',').ToList();

                    string name = parameters.name.ToString();

                    var finish = new List<ServiceInfo>();

                    ServiceController[] services = ServiceController.GetServices();
                    var service = services.FirstOrDefault(s => s.ServiceName.ToLower() == name.ToLower());

                    if (service != null && !_services.Contains(name.ToLower()))
                    {
                        var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        var setting = configFile.AppSettings.Settings["SERVICES"];

                        var items = setting.Value.Split(',').ToList();
                        items.Add(name);

                        var stringList = String.Join(",", items.ToArray());

                        setting.Value = stringList;

                        configFile.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                    }
                    else
                    {
                        return Response.AsJson(new ServiceResponse() { Status = "Warning", Message = "Service already added" });
                    }


                    return Response.AsJson(new ServiceResponse() { Status = "OK", Message = "Service added" });
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new ServiceResponse() { Status = "Error", Message = ex.Message });
                }
            };

            Get["/v1/admin/service/remove/{name}"] = parameters =>
            {
                try
                {
                    var _services = ConfigurationManager.AppSettings["SERVICES"].ToLower().Split(',').ToList();

                    string name = parameters.name.ToString();

                    ServiceController[] services = ServiceController.GetServices();
                    var service = services.FirstOrDefault(s => s.ServiceName.ToLower() == name.ToLower());

                    if (service != null && _services.Contains(name.ToLower()))
                    {
                        var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                        var setting = configFile.AppSettings.Settings["SERVICES"];

                        var items = setting.Value.ToLower().Split(',').ToList();
                        items.Remove(name);

                        var stringList = String.Join(",", items.ToArray());

                        setting.Value = stringList;

                        configFile.Save(ConfigurationSaveMode.Modified);
                        ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
                    }
                    else
                    {
                        return Response.AsJson(new ServiceResponse() { Status = "Warning", Message = "Service not found" });
                    }

                    return Response.AsJson(new ServiceResponse() { Status = "OK", Message = "Service removed" });
                }
                catch (Exception ex)
                {
                    return Response.AsJson(new ServiceResponse() { Status = "Error", Message = ex.Message });
                }
            };
        }
    }
}