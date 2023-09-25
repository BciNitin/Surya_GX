using ELog.Core;
using ELog.Web.Core.Auditing;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;
using System.IO;

namespace ELog.Web.Host.Startup
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Log.Logger = SerilLogDbInitializer.CreateLoggerConfiguration().CreateLogger();

            var file = File.CreateText(PMMSConsts.SerilogSelfLogPath);
            Serilog.Debugging.SelfLog.Enable(_ =>
            {
                TextWriter.Synchronized(file);
                //Debug.Print(msg);
                //Debugger.Break();
            });

            try
            {
                Log.Information("Application Starting.");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "The Application failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            return WebHost.CreateDefaultBuilder(args).UseSerilog()
                .UseStartup<Startup>()
                .Build();
        }
    }
}