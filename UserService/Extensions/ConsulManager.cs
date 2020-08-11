using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Extensions
{
    public static class ConsulManager
    {
        //private readonly IConsulClient _client;
        //public ConsulManager(IConsulClient client)
        //{
        //    _client = client;
        //}
        public static void UseConsul(this IApplicationBuilder app, IConsulClient client)
        {
              RegServer(client);
        }

        private static void RegServer(IConsulClient client)
        {
            var consulGroupName = "UserSevice";

            var ip = "localhost";

            var port = 5001;

            var serviceId = $"{consulGroupName}_{ip}_{port}";
            var regist = new AgentServiceRegistration()
            {
                Address = ip,
                Port = port,
                Name = consulGroupName,
                ID = serviceId,
                Check=new AgentServiceCheck() { 
                    Interval=TimeSpan.FromSeconds(20), //pinlv
                    HTTP=$"http://{ip}:{port}/heart",  //xintiao url
                    Timeout=TimeSpan.FromSeconds(2),
                    DeregisterCriticalServiceAfter=TimeSpan.FromSeconds(2)
                }
            };

            client.Agent.ServiceRegister(regist);
        }
    }
}
