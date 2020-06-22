using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.IO;

namespace ForDotNet.Auth
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureLogging(builder => 
                    {
                        builder.ClearProviders();
                    });
                });
                //.UseSerilog((context, configuration) =>
                //{
                //    configuration
                //    .MinimumLevel.Information()
                //    .Enrich.FromLogContext()
                //    .WriteTo.Console()
                //    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                //    {
                //        MinimumLogEventLevel = Serilog.Events.LogEventLevel.Verbose,
                //        AutoRegisterTemplate = true
                //    }); ;
                //});
    }
}