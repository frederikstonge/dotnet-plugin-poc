using PluginBase;
using WebshopTests.Data;
using Weikio.NugetDownloader;
using Weikio.PluginFramework.Catalogs;
using Weikio.PluginFramework.Catalogs.NuGet;

namespace WebshopTests
{
    public class Program
    {
        public const string PluginPath = "C:\\temp";
        private static readonly IApplicationLifetime _applicationLifetime = new ApplicationLifetime();

        public static async Task Main(string[] args)
        {
            var files = Directory.GetFiles(PluginPath, "*.nupkg", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                File.Delete(file);
            }

            while (_applicationLifetime.StartRequest)
            {
                try
                {
                    _applicationLifetime.Start();
                    await StartAsync(args);
                }
                catch (OperationCanceledException e)
                {
                    Console.WriteLine(e);
                }
            }
        }

        public static async Task StartAsync(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton(_applicationLifetime);

            // Plugin start

            var feed = new NuGetFeed("local feed", PluginPath);

            var nugetCatalog = new NugetFeedPluginCatalog(
                feed,
                options: new NugetFeedPluginCatalogOptions()
                {
                    ForcePackageCaching = false,
                },
                configureFinder: finder => 
                {
                    finder.Implements<IPlugin>();
                });

            builder.Services.AddPluginFramework()
                .AddPluginCatalog(nugetCatalog)
                .AddPluginType<IPlugin>();

            // Plugin end

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            await app.RunAsync(_applicationLifetime.CancellationTokenSource!.Token);
        }
    }
}