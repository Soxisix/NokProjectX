using System;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Views;
using NokProjectX.Wpf.Views.Transaction;

namespace NokProjectX.Wpf.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(YumiContext context)
        {
            _context = context;
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
//            SideBarView = ServiceLocator.Current.GetInstance<SideBarViewModel>();
            MessengerInstance.Register<NavigateTo>(this, NavigateToContent);
            TopBarView = new TopBarView();
            SideBarView = new SideBarView();
            MainView = new TransactionView();
        }

        public void NavigateToContent(NavigateTo content)
        {
            MainView = content.Content;
        }

        private object topBarView;
        public object TopBarView { get { return topBarView; } set { Set(ref topBarView, value); } }

        private object sideBarView;
        public object SideBarView { get { return sideBarView; } set { Set(ref sideBarView, value); } }

        private object mainView;
        private readonly YumiContext _context;

        public object MainView { get { return mainView; } set { Set(ref mainView, value); } }
    }
}