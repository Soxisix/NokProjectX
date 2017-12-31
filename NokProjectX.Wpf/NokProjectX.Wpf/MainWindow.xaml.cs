using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.ViewModel;
using NokProjectX.Wpf.Views.UserLogin;

namespace NokProjectX.Wpf
{
    using System.Windows;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<SignInOutMessage>(this, OnSignInOut);
            
        }

        private void OnSignInOut(SignInOutMessage obj)
        {
            var main = ServiceLocator.Current.GetInstance<MainViewModel>();
            main.Cleanup();
            LoginView login = new LoginView();
            login.Show();Close();
        }

        private void MainWindow_OnUnloaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.Cleanup();
        }
    }
}
