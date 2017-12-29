using NokProjectX.Wpf.ViewModel.Reports;
using NokProjectX.Wpf.ViewModel.UserLogin;
using NokProjectX.Wpf.Views.Reports;

namespace NokProjectX.Wpf.ViewModel
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using Microsoft.Practices.ServiceLocation;
    using NokProjectX.Wpf.Context;
    using NokProjectX.Wpf.ViewModel.Common;
    using NokProjectX.Wpf.ViewModel.Inventory;
    using NokProjectX.Wpf.ViewModel.Transaction;
    using NokProjectX.Wpf.ViewModel.Settings;
    using NokProjectX.Wpf.ViewModel.About;

    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (ViewModelBase.IsInDesignModeStatic)
            {

                // Create design time view services and models
                SimpleIoc.Default.Register<YumiContext>();
            }
            else

            {
                // Create run time view services and models
                SimpleIoc.Default.Register<YumiContext>();
            }


            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SideBarViewModel>();
            SimpleIoc.Default.Register<TopBarViewModel>();
            SimpleIoc.Default.Register<InventoryViewModel>();
            SimpleIoc.Default.Register<AddProductViewModel>();
            SimpleIoc.Default.Register<EditProductViewModel>();
            SimpleIoc.Default.Register<AddStockViewModel>();SimpleIoc.Default.Register<TransactionViewModel>();
            SimpleIoc.Default.Register<LoginViewModel>();
            SimpleIoc.Default.Register<ReportViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();
            SimpleIoc.Default.Register<AddUserAccountViewModel>();
            SimpleIoc.Default.Register<AddCustomerViewModel>();
            SimpleIoc.Default.Register<EditCustomerViewModel>();
            SimpleIoc.Default.Register<EditUserAccountViewModel>();
            SimpleIoc.Default.Register<AboutViewModel>();
        }

        #endregion

        #region Properties
        public SettingsViewModel SettingsView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }

        public AboutViewModel AboutView{
            get
            {
                return ServiceLocator.Current.GetInstance<AboutViewModel>();
            }
        }
        public AddUserAccountViewModel AddUser
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddUserAccountViewModel>();
            }
        }

        public AddCustomerViewModel AddCustomer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddCustomerViewModel>();
            }
        }

        public EditCustomerViewModel EditCustomer
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditCustomerViewModel>();
            }
        }
        public EditUserAccountViewModel EditUser
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditUserAccountViewModel>();
            }
        }
        /// <summary>
        /// Gets the AddProduct
        /// </summary>
        public AddProductViewModel AddProduct
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddProductViewModel>();
            }
        }

        /// <summary>
        /// Gets the AddStock
        /// </summary>
        public AddStockViewModel AddStock
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddStockViewModel>();
            }
        }

        /// <summary>
        /// Gets the EditProduct
        /// </summary>
        public EditProductViewModel EditProduct
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditProductViewModel>();
            }
        }

        /// <summary>
        /// Gets the Inventory
        /// </summary>
        public InventoryViewModel Inventory
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InventoryViewModel>();
            }
        }

        /// <summary>
        /// Gets the Settings
        /// </summary>

        public  SettingsViewModel Settings
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SettingsViewModel>();
            }
        }



        /// <summary>
        /// Gets the Main
        /// </summary>
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }

        /// <summary>
        /// Gets the SideBar
        /// </summary>
        public SideBarViewModel SideBar
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SideBarViewModel>();
            }
        }

        /// <summary>
        /// Gets the TopBar
        /// </summary>
        public TopBarViewModel TopBar
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TopBarViewModel>();
            }
        }

        /// <summary>
        /// Gets the Transaction
        /// </summary>
        public TransactionViewModel Transaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionViewModel>(); }

        }
      public LoginViewModel LoginView
            {
                get
                {
                    return ServiceLocator.Current.GetInstance<LoginViewModel>();
                }
            }
        public ReportViewModel ReportView
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ReportViewModel>();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The Cleanup
        /// </summary>
        public static void Cleanup()
        {
            SimpleIoc.Default.Unregister<YumiContext>();
            SimpleIoc.Default.Unregister<MainViewModel>();
            SimpleIoc.Default.Unregister<SideBarViewModel>();
            SimpleIoc.Default.Unregister<TopBarViewModel>();
            SimpleIoc.Default.Unregister<InventoryViewModel>();
            SimpleIoc.Default.Unregister<AddProductViewModel>();
            SimpleIoc.Default.Unregister<EditProductViewModel>();
            SimpleIoc.Default.Unregister<AddStockViewModel>();
            SimpleIoc.Default.Unregister<TransactionViewModel>();
            SimpleIoc.Default.Unregister<LoginViewModel>();
            SimpleIoc.Default.Unregister<ReportViewModel>();
        }

        #endregion
    }
}
