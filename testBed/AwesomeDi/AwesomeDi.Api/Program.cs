using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

namespace AwesomeDi.Api
{
	public class Program
	{
		public static void Main(string[] args)
		{
#if !DEBUG
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Logger(c =>
                    c.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                        .WriteTo.File(new CompactJsonFormatter(), $"log-Err.json", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30))
				.WriteTo.File(new CompactJsonFormatter(), $"log.json", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 30)
                .CreateLogger();
#endif
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
#if !DEBUG
				.ConfigureLogging(logging =>
                {
                    logging.AddSerilog();
                })
#endif
                .ConfigureWebHostDefaults(webBuilder =>
				{
					webBuilder.UseStartup<Startup>();
				});
	}
}
