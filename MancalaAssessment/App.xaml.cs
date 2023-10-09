using CommunityToolkit.Mvvm.Messaging;
using MancalaGame;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;

namespace MancalaWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly IServiceProvider ServiceProvider = ConfigureServices();

        public App()
        {
            this.InitializeComponent();
        }

        public static T GetService<T>()
        {
            return ServiceProvider.GetService<T>()!;
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddTransient<IMancala, Mancala>();
            services.AddSingleton<IMessenger, StrongReferenceMessenger>();

            return services.BuildServiceProvider();
        }
    }
}
