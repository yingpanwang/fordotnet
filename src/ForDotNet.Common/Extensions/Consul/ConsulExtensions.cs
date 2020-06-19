using Consul;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace ForDotNet.Common.Consul.Extensions
{
    /// <summary>
    /// 服务配置信息
    /// </summary>
    public class ServiceOptions
    {
        /// <summary>
        /// 服务ip
        /// </summary>
        public string ServiceIP { get; set; }

        /// <summary>
        /// 服务名称
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// 协议类型http or https
        /// </summary>
        public string Scheme { get; set; } = "http";

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 健康检查接口
        /// </summary>
        public string HealthCheckUrl { get; set; } = "/api/values";

        /// <summary>
        /// 健康检查间隔时间
        /// </summary>
        public int HealthCheckIntervalSecond { get; set; } = 10;

        /// <summary>
        /// consul配置信息
        /// </summary>
        public ConsulOptions ConsulOptions { get; set; }
    }

    /// <summary>
    /// consul配置信息
    /// </summary>
    public class ConsulOptions
    {
        /// <summary>
        /// consul ip
        /// </summary>
        public string ConsulIP { get; set; }

        /// <summary>
        /// consul 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 协议类型http or https
        /// </summary>
        public string Scheme { get; set; } = "http";
    }

    /// <summary>
    /// consul注册客户端信息
    /// </summary>
    public class ConsulClientInfo
    {
        /// <summary>
        /// 注册信息
        /// </summary>
        public AgentServiceRegistration RegisterInfo { get; set; }

        /// <summary>
        /// consul客户端
        /// </summary>
        public ConsulClient Client { get; set; }
    }

    /// <summary>
    /// consul扩展(通过配置文件配置)
    /// </summary>
    public static class ConsulExtensions
    {
        private static readonly ServiceOptions serviceOptions = new ServiceOptions();

        /// <summary>
        /// 添加consul
        /// </summary>
        public static void AddConsulServiceDiscovery(this IServiceCollection services)
        {
            var config = services.BuildServiceProvider().GetService<IConfiguration>();
            config.GetSection("ServiceOptions").Bind(serviceOptions);
            //config.Bind(serviceOptions);

            if (serviceOptions == null)
            {
                throw new Exception("获取服务注册信息失败!请检查配置信息是否正确!");
            }
            Register(services);
        }

        /// <summary>
        /// 添加consul(通过配置opt对象配置)
        /// </summary>
        /// <param name="app"></param>
        /// <param name="life">引用生命周期</param>
        /// <param name="options">配置参数</param>
        public static void AddConsulServiceDiscovery(this IServiceCollection services, Action<ServiceOptions> options)
        {
            options.Invoke(serviceOptions);
            Register(services);
        }

        /// <summary>
        /// 注册consul服务发现
        /// </summary>
        /// <param name="app"></param>
        /// <param name="life"></param>
        public static void UseConsulServiceDiscovery(this IApplicationBuilder app, IHostApplicationLifetime life)
        {
            var consulClientInfo = app.ApplicationServices.GetRequiredService<ConsulClientInfo>();
            if (consulClientInfo != null)
            {
                life.ApplicationStarted.Register( () =>
                {
                     consulClientInfo.Client.Agent.ServiceRegister(consulClientInfo.RegisterInfo).Wait();
                });

                life.ApplicationStopping.Register( () =>
                {
                     consulClientInfo.Client.Agent.ServiceDeregister(consulClientInfo.RegisterInfo.ID).Wait();
                });
            }
            else
            {
                throw new NullReferenceException("未找到相关consul客户端信息!");
            }
        }

        private static void Register(this IServiceCollection services)
        {
            if (serviceOptions == null)
            {
                throw new Exception("获取服务注册信息失败!请检查配置信息是否正确!");
            }
            if (serviceOptions.ConsulOptions == null)
            {
                throw new ArgumentNullException("请检查是否配置Consul信息！");
            }

            string consulAddress = $"{serviceOptions.ConsulOptions.Scheme}://{serviceOptions.ConsulOptions.ConsulIP}:{serviceOptions.ConsulOptions.Port}";

            var consulClient = new ConsulClient(opt =>
            {
                opt.Address = new Uri(consulAddress);
            });

            var httpCheck = new AgentServiceCheck()
            {
                DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(10), // 服务启动多久后注册
                Interval = TimeSpan.FromSeconds(serviceOptions.HealthCheckIntervalSecond), // 间隔
                HTTP = $"{serviceOptions.Scheme}://{serviceOptions.ServiceIP}:{serviceOptions.Port}{serviceOptions.HealthCheckUrl}",
                Timeout = TimeSpan.FromSeconds(10)
            };

            var registration = new AgentServiceRegistration()
            {
                Checks = new[] { httpCheck },
                ID = Guid.NewGuid().ToString(),
                Name = serviceOptions.ServiceName,
                Address = serviceOptions.ServiceIP,
                Port = serviceOptions.Port,
            };

            services.AddSingleton(new ConsulClientInfo()
            {
                Client = consulClient,
                RegisterInfo = registration
            });
        }
    }
}