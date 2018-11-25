﻿using Autofac;
using BookOrganizer.UI.WPF.Startup;
using System.Windows;

namespace BookOrganizer.UI.WPF
{
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var bootstrapper = new Bootstrapper();
            var container = bootstrapper.Bootstrap();

            var mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();
        }

    }
}
