// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.IO;
using Softeq.NetKit.Auth.Common.Logger;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace Softeq.NetKit.Auth.Web
{
	public class Program
	{
		public static void Main(string[] args)
		{
			BuildWebHost(args).Run();
		}

		public static IWebHost BuildWebHost(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseContentRoot(Directory.GetCurrentDirectory())
				.UseApplicationInsights()
			    .UseLogger(GetLoggerConfiguration)
				.UseStartup<Startup>()
				.Build();

	    private static LoggerConfiguration GetLoggerConfiguration(WebHostBuilderContext context)
	    {
	        return new LoggerConfiguration(
	            context.Configuration["ApplicationInsights:InstrumentationKey"],
	            context.Configuration["Serilog:ApplicationName"],
	            bool.Parse(context.Configuration["Serilog:EnableLocalFileSink"]),
	            int.Parse(context.Configuration["Serilog:FileSizeLimitMBytes"]));
	    }
    }
}
