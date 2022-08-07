using MeanSongs;
using MeanSongs.Services;
using MeanSongs.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Serilog;

public class Program
{
    async static Task Main(string[] args)
    {
        // Setup Host
        var host = createDefaultBuilder().Build();

        // Invoke Worker
        using (IServiceScope serviceScope = host.Services.CreateScope()) 
        {
            IServiceProvider provider = serviceScope.ServiceProvider;
            var workerInstance = provider.GetRequiredService<MeanWorker>();

            try
            {
                string? input = null;
                do
                {
                    await workerInstance.Execute();
                    Console.WriteLine("Would you like to run again (Y or Yes)?");
                    input = Console.ReadLine();
                } 
                while (!string.IsNullOrWhiteSpace(input) && (input.Trim().Equals("Y", StringComparison.InvariantCultureIgnoreCase) || input.Trim().Equals("Yes", StringComparison.InvariantCultureIgnoreCase)));
            }
            catch (Exception)
            {
                Console.WriteLine("Something has gone wrong. Closing Application");
            }
        }
    }

    static IHostBuilder createDefaultBuilder()
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build(); 
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(config)
            .CreateLogger();

        return Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration(app =>
            {
                app.AddJsonFile("appsettings.json");
            })
            .ConfigureServices(services =>
            {
                services.AddSingleton<ICommandLineService, CommandLineService>();
                services.AddHttpClient<ImusicbrainzService, MusicbrainzService>();
                services.AddSingleton<ImusicbrainzService, MusicbrainzService>();
                services.AddHttpClient<ILyricsovh, LyricsovhService>();
                services.AddSingleton<ILyricsovh, LyricsovhService>();
                services.AddSingleton<MeanWorker>();

                services.RemoveAll<IHttpMessageHandlerBuilderFilter>();
            });
    }
}