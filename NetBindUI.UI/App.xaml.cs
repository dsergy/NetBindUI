using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NetBindUI.Contracts.Interfaces.Network;
using NetBindUI.Contracts.Interfaces.Process;
using NetBindUI.Core.Services;
using NetBindUI.UI.ViewModels;
using System;
using System.Windows;

namespace NetBindUI.UI
{
    public partial class App : Application
    {
        private static IHost? _host;

        public static IHost Host => _host ??= Microsoft.Extensions.Hosting.Host
            .CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<INetworkService, NetworkService>();
                services.AddSingleton<IProcessService, ProcessService>();

                services.AddSingleton<MainViewModel>();
                services.AddTransient<MainWindow>();
            })
            .Build();

        public static T GetService<T>() where T : class => Host.Services.GetRequiredService<T>();

        protected override async void OnStartup(StartupEventArgs e)
        {
            await Host.StartAsync();

            var mainWindow = GetService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            using (Host)
            {
                await Host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}