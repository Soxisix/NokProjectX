using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using NokProjectX.Wpf.Views;

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
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
//            SideBarView = ServiceLocator.Current.GetInstance<SideBarViewModel>();
            TopBarView = new TopBarView();
            SideBarView = new SideBarView();
        }

        private object topBarView;
        public object TopBarView { get { return topBarView; } set { Set(ref topBarView, value); } }

        private object sideBarView;
        public object SideBarView { get { return sideBarView; } set { Set(ref sideBarView, value); } }
    }
}