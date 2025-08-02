using EnvironmentVariableExplorer.Components;
using EnvironmentVariableExplorer.Services;
using EnvironmentVariableExplorer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using MudBlazor;
using MudBlazor.Services;
using Photino.Blazor;
using System;
using System.IO;
using System.Reflection;
using System.Globalization;

namespace EnvironmentVariableExplorer
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            PhotinoBlazorAppBuilder appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

            ConfigureServices(appBuilder.Services);

            appBuilder.RootComponents.Add<App>("app");

            PhotinoBlazorApp app = appBuilder.Build();

            string iconPath = ExtractEmbeddedResourceToTempFile("favicon.ico");

            UserPreferenceService userPreferenceService = new UserPreferenceService();
            CultureInfo preferredCulture = userPreferenceService.GetUserLanguagePreferenceAsync().GetAwaiter().GetResult();
            CultureInfo.DefaultThreadCurrentCulture = preferredCulture;
            CultureInfo.DefaultThreadCurrentUICulture = preferredCulture;
            string localizedAppTitle = Resources.Strings.AppTitle + " "; // Needs a space a the end of the string due to Windows 11 taskbar icon bug

            app.MainWindow
                .SetSize(1280, 800)
                .SetLogVerbosity(0)
                .SetDevToolsEnabled(SystemUtils.IsDevelopment)
                .SetContextMenuEnabled(SystemUtils.IsDevelopment)
                .SetIconFile(iconPath)
                .SetTitle(localizedAppTitle);

            AppDomain.CurrentDomain.UnhandledException += (sender, error) =>
            {
                app.MainWindow.ShowMessage("Fatal exception", error.ExceptionObject.ToString());
            };

            app.Run();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            services.AddSingleton<IFileProvider>(_ => new ManifestEmbeddedFileProvider(typeof(Program).Assembly, "wwwroot"));
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            services.AddSingleton<LanguageService>();
            services.AddSingleton<UserPreferenceService>();
            services.AddSingleton<EnvironmentVariableService>();
            services.AddMudServices(config =>
            {
                config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
            });
        }

        private static string ExtractEmbeddedResourceToTempFile(string fileName)
        {
            string resourceNamespace = typeof(Program).Namespace;
            string resourceName = $"{resourceNamespace}.wwwroot.{fileName}";

            Assembly assembly = Assembly.GetExecutingAssembly();

            using (Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    Console.WriteLine($"Resource {resourceName} not found.");
                    return null;
                }

                string tempFile = Path.Combine(Path.GetTempPath(), fileName);

                using (FileStream fileStream = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                {
                    resourceStream.CopyTo(fileStream);
                }

                return tempFile;
            }
        }
    }
}
