using ForDotNet.Auth.Config;
using ForDotNet.Common.Consul.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;

namespace ForDotNet.Auth
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddIdentityServer() // ע��IdentityServer4
                .AddDeveloperSigningCredential() // ���ÿ�����ƾ֤
                .AddInMemoryApiResources(IdentityServerConfig.GetApiResources()) // ���Api��Դ
                .AddInMemoryClients(IdentityServerConfig.GetClients()) // ��ӿͻ���
                .AddInMemoryIdentityResources(IdentityServerConfig.GetIdentityResources()) // ��������Դ
                .AddResourceOwnerValidator<MyResourceOwnerValidator>();
                //.AddTestUsers(new List<IdentityServer4.Test.TestUser>() // ��Ӳ����û�
                //{
                //     new IdentityServer4.Test.TestUser ()
                //     {
                //         Username = "admin",
                //         Password = "123",
                //         SubjectId = "999"
                //     }
                //});

            // ע������ַ���
            services.AddConsulServiceDiscovery(); 

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime life)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ע��IdentityServer
            app.UseIdentityServer();

            // ע�������
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