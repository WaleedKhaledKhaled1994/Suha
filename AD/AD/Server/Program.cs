using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;
using Serilog.Sinks.MSSqlServer.Sinks.MSSqlServer.Options;
using System.IO;
using System.Collections.ObjectModel;
using System.Data;

namespace AD.Server
{
    public class Program
    {
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
    .Build();

        [Obsolete]
        public static void Main(string[] args)
        {
            string connectionString = Configuration.GetConnectionString("SqlConnLogging");

            var columnOptions = new ColumnOptions
            {
                AdditionalColumns = new Collection<SqlColumn>
                {
                    new SqlColumn("UserName",SqlDbType.VarChar)
                }
            };

            //Log.Logger = new LoggerConfiguration()
            //    .Enrich.FromLogContext()
            //    .WriteTo.File(@"D:\Developer\SABX\Ali Dantel\4\AD\AD\logs\log.txt",
            //    restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Error,
            //    outputTemplate: "[{Level:u3}] - {Timestamp:dd-MM-yyyy HH:mm:ss}{Message:lj}{NewLine}{Exception}")
            //    .CreateLogger();

            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.MSSqlServer(connectionString,
                sinkOptions: new SinkOptions { TableName = "WebApiLogs" }
                , null, null, LogEventLevel.Information, null, columnOptions: columnOptions, null, null)
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("Hangfire", LogEventLevel.Warning)
                .CreateLogger();

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
