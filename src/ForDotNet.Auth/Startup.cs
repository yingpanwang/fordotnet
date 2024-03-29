using ForDotNet.Auth.Config;
using ForDotNet.Common.Consul.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGeneration.DotNet;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.File;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace ForDotNet.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IHostEnvironment hostEnvironment)
        {

           // 读取配置文件
           var builder = new ConfigurationBuilder()
               .SetBasePath(hostEnvironment.ContentRootPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
               .AddEnvironmentVariables();

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(Configuration)
                    .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddIdentityServer() // 注册IdentityServer4
                .AddDeveloperSigningCredential() // 采用开发者凭证
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources()) // 添加Api资源
                .AddInMemoryClients(IdentityServerConfig.GetClients()) // 添加客户端
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources()) // 添加身份资源
                .AddResourceOwnerValidator<MyResourceOwnerValidator>();
                //.AddTestUsers(new List<IdentityServer4.Test.TestUser>() // 添加测试用户
                //{
                //     new IdentityServer4.Test.TestUser ()
                //     {
                //         Username = "admin",
                //         Password = "123",
                //         SubjectId = "999"
                //     }
                //});

            // 注册服务发现服务
            services.AddConsulServiceDiscovery(); 

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,ILoggerFactory loggerFactory, IHostApplicationLifetime life)
        {
            loggerFactory.AddSerilog();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // 注册IdentityServer
            app.UseIdentityServer();

            // 注册服务发现
            app.UseConsulServiceDiscovery(life);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}