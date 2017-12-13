
using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.ViewModel.Common;
using NokProjectX.Wpf.ViewModel.Inventory;
using System.Windows;
using NokProjectX.Wpf.ViewModel.Transaction;

namespace NokProjectX.Wpf.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
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
            SimpleIoc.Default.Register<AddStockViewModel>();
            SimpleIoc.Default.Register<TransactionViewModel>();
        }
        
        
        
        public MainViewModel Main
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public SideBarViewModel SideBar
        {
            get
            {
                return ServiceLocator.Current.GetInstance<SideBarViewModel>();
            }
        }
        public TopBarViewModel TopBar
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TopBarViewModel>();
            }
        }
        public InventoryViewModel Inventory
        {
            get
            {
                return ServiceLocator.Current.GetInstance<InventoryViewModel>();
            }
        }
        public AddProductViewModel AddProduct
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddProductViewModel>();
            }
        }
        public EditProductViewModel EditProduct
        {
            get
            {
                return ServiceLocator.Current.GetInstance<EditProductViewModel>();
            }
        }
        public AddStockViewModel AddStock
        {
            get
            {
                return ServiceLocator.Current.GetInstance<AddStockViewModel>();
            }
        }
        public TransactionViewModel Transaction
        {
            get
            {
                return ServiceLocator.Current.GetInstance<TransactionViewModel>();
            }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}