using App.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.IO;

// DI, Serilog, Settings

namespace ConsoleUI {
	class Program {
		static void Main(string[] args) {
			IConfigurationRoot configuration = GetConfiguraiton(args);
			Log.Logger = GetLogger(configuration);
			Log.Logger.Information("Application Starting");
			IHostBuilder hostBuilder = Host.CreateDefaultBuilder()
										   .ConfigureServices(ConfigureServices);
			Configure(hostBuilder);
			IHost host = hostBuilder.Build();
			IServiceProvider services = host.Services;
			// IGreetingService svc = ActivatorUtilities.CreateInstance<GreetingService>(services);
			IGreetingService svc = services.GetRequiredService<IGreetingService>();
			svc.Run();
		}

		static ILogger GetLogger(IConfigurationRoot configuration) =>
			 new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Enrich.FromLogContext()
				.WriteTo.Console()
				.CreateLogger();

		static IConfigurationRoot GetConfiguraiton(string[] args) {
			string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
			return new ConfigurationBuilder()
				.SetBasePath(Directory.GetCurrentDirectory())
				.AddCommandLine(args)
				.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
				.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
				.AddEnvironmentVariables()
				.Build();
		}

		static void ConfigureServices(HostBuilderContext context, IServiceCollection services) {
			services.AddTransient<IGreetingService, GreetingService>();
		}

		static void Configure(IHostBuilder hostBuilder) {
			hostBuilder.UseSerilog();
		}
	}
}
