namespace NokProjectX.Wpf.ViewModel
{
    using GalaSoft.MvvmLight;
    using NokProjectX.Wpf.Common.Messages;
    using NokProjectX.Wpf.Context;
    using NokProjectX.Wpf.Views.Transaction;
    using SideBarView = NokProjectX.Wpf.Views.Common.SideBarView;
    using TopBarView = NokProjectX.Wpf.Views.Common.TopBarView;

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
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the mainView
        /// </summary>
        private object mainView;

        /// <summary>
        /// Defines the sideBarView
        /// </summary>
        private object sideBarView;

        /// <summary>
        /// Defines the topBarView
        /// </summary>
        private object topBarView;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(YumiContext context)
        {
            _context = context;
            //            if (IsInDesignMode)
            //            {
            //                // Code runs in Blend --> create design time data.
            //            }
            //            else
            //            {
            //                // Code runs "for real"
            //            }
            //            SideBarView = ServiceLocator.Current.GetInstance<SideBarViewModel>();
            MessengerInstance.Register<NavigateTo>(this, NavigateToContent);
            TopBarView = new TopBarView();
            SideBarView = new SideBarView();
            MainView = new TransactionView();
        }

        /// <summary>
        /// Gets or sets the MainView
        /// </summary>
        public object MainView
        {
            get { return mainView; }
            set { Set(ref mainView, value); }
        }

        /// <summary>
        /// Gets or sets the SideBarView
        /// </summary>
        public object SideBarView
        {
            get { return sideBarView; }
            set { Set(ref sideBarView, value); }
        }

        /// <summary>
        /// Gets or sets the TopBarView
        /// </summary>
        public object TopBarView
        {
            get { return topBarView; }
            set { Set(ref topBarView, value); }
        }

        /// <summary>
        /// The NavigateToContent
        /// </summary>
        /// <param name="content">The <see cref="NavigateTo"/></param>
        public void NavigateToContent(NavigateTo content)
        {
            MainView = content.Content;
        }
    }
}
