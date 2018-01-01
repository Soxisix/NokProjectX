using System;
using System.IO;
using System.Reflection;
using DevExpress.Mvvm.Native;
using MaterialDesignThemes.Wpf;
using NokProjectX.Wpf.Views.UserLogin;

namespace NokProjectX.Wpf
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            //
            //            MainWindow w1 = new MainWindow(); // main win
            //            Application.Current.MainWindow = w1;
            //
            //            LoginView w = new LoginView(); // your license win
            //            w.ShowDialog();
            //
            //            w1.Show();
            LoadPlugins();
            base.OnStartup(e);
            
        }
        static void LoadPlugins()
        {
            foreach (string file in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments), "DevExpress.HybridApp.Wpf.Plugins.*.exe"))
            {
                Assembly.LoadFrom(file)
                    .With(x => x.GetType("Global.Program"))
                    .With(x => x.GetMethod("Start", BindingFlags.Static | BindingFlags.Public, null, new Type[] { }, null))
                    .Do(x => x.Invoke(null, new object[] { }));
            }
        }
    }
}
