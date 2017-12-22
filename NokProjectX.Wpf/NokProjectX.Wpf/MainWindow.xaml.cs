using GalaSoft.MvvmLight.Messaging;
using Microsoft.Practices.ServiceLocation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.ViewModel;

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
            Close();
        }
    }
}
