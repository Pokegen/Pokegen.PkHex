using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Pokégen.PkHex.Util;
using Serilog;
using Serilog.Enrichers;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace Pokégen.PkHex
{
	public class Program
	{
		public static void Main(string[] args)
			=> CreateHostBuilder(args).Build().Run();

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseSerilog((_, _, loggerConfiguration) =>
				{
					loggerConfiguration
						.MinimumLevel.Is(LoggingUtil.IsDevelopment ? LogEventLevel.Debug : LogEventLevel.Information)
						.MinimumLevel.Override("Microsoft", LogEventLevel.Information)
						.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
						.Enrich.WithThreadId()
						.Enrich.WithThreadName()
						.Enrich.WithProperty(ThreadNameEnricher.ThreadNamePropertyName, "Main")
						.WriteTo.Console(
							outputTemplate:
							"[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{ThreadName}] {Message:lj}\n{Exception}",
							theme: SystemConsoleTheme.Colored);
				}, writeToProviders: true)
				.ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
	}
}