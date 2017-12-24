namespace NokProjectX.Wpf.ViewModel.Inventory
{
    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.CommandWpf;
    using MaterialDesignThemes.Wpf;
    using Microsoft.Practices.ServiceLocation;
    using NokProjectX.Wpf.Common.Messages;
    using NokProjectX.Wpf.Context;
    using NokProjectX.Wpf.Entities;
    using NokProjectX.Wpf.Views.Common;
    using NokProjectX.Wpf.Views.Inventory;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;

    /// <summary>
    /// Defines the <see cref="InventoryViewModel" />
    /// </summary>
    public class InventoryViewModel : ViewModelBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the _productList
        /// </summary>
        private List<Product> _productList;

        /// <summary>
        /// Defines the _searchText
        /// </summary>
        private string _searchText;

        /// <summary>
        /// Defines the _selectedProduct
        /// </summary>
        private Product _selectedProduct;

        /// <summary>
        /// Defines the _totalCount
        /// </summary>
        private int _totalCount;

        /// <summary>
        /// Defines the OriginalProductList
        /// </summary>
        private List<Product> OriginalProductList;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public InventoryViewModel(YumiContext context)
        {
            _context = context;
            MessengerInstance.Register<RefreshMessage>(this, DoRefresh);
            LoadData();

            LoadCommands();
        }

        /// <summary>
        /// Gets or sets the AddProductCommand
        /// </summary>
        public RelayCommand AddProductCommand { get; set; }

        /// <summary>
        /// Gets or sets the BatchAddStockCommand
        /// </summary>
        public RelayCommand BatchAddStockCommand { get; set; }

        /// <summary>
        /// Gets or sets the BatchDeleteCommand
        /// </summary>
        public RelayCommand BatchDeleteCommand { get; set; }

        /// <summary>
        /// Gets or sets the CloseCommand
        /// </summary>
        public RelayCommand CloseCommand { get; set; }

        /// <summary>
        /// Gets or sets the DeleteProductCommand
        /// </summary>
        public RelayCommand DeleteProductCommand { get; set; }

        /// <summary>
        /// Gets or sets the EditProductCommand
        /// </summary>
        public RelayCommand EditProductCommand { get; set; }

        /// <summary>
        /// Gets the ListOfProducts
        /// </summary>
        public List<Product> ListOfProducts
        {
            get { return ProductList.Where(c => c.IsSelected == true).ToList(); }
        }

        /// <summary>
        /// Gets or sets the ProductList
        /// </summary>
        public List<Product> ProductList
        {
            get { return _productList; }
            set { Set(ref _productList, value); }
        }

        /// <summary>
        /// Gets or sets the SearchText
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                Set(ref _searchText, value);
                if (String.IsNullOrEmpty(SearchText))
                {
                    ProductList = OriginalProductList;
                }
                else
                {
                    ProductList = OriginalProductList.Where(c =>
                            c.Name.ToLower().Contains(SearchText.Trim().ToLower()) ||
                            SearchText.Trim().Contains(c.ProductCode.ToString()) ||
                            c.Description.ToLower().Contains(SearchText.Trim().ToLower()))
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Gets or sets the SelectedProduct
        /// </summary>
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { Set(ref _selectedProduct, value); }
        }

        /// <summary>
        /// Gets or sets the TotalCount
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            set { Set(ref _totalCount, value); }
        }

        /// <summary>
        /// Gets or sets the ViewProductCommand
        /// </summary>
        public RelayCommand ViewProductCommand { get; set; }

        /// <summary>
        /// The DoRefresh
        /// </summary>
        /// <param name="obj">The <see cref="RefreshMessage"/></param>
        private void DoRefresh(RefreshMessage obj)
        {
            LoadData();
        }

        /// <summary>
        /// The LoadCommands
        /// </summary>
        private void LoadCommands()
        {
            CloseCommand = new RelayCommand(OnClose);

            ViewProductCommand = new RelayCommand(OnView);
            AddProductCommand = new RelayCommand(OnAddProduct);
            EditProductCommand = new RelayCommand(OnEdit);
            DeleteProductCommand = new RelayCommand(OnDelete);
            BatchAddStockCommand = new RelayCommand(OnBatchAddStock, () => (ProductList.Count(c => c.IsSelected) > 0));
            BatchDeleteCommand = new RelayCommand(OnBatchDelete, () =>
                (ProductList.Count(c => c.IsSelected) > 0)
            );
        }

        /// <summary>
        /// The LoadData
        /// </summary>
        private void LoadData()
        {
            OriginalProductList = _context.Products.ToList();
            ProductList = OriginalProductList;
            TotalCount = ProductList.Count;
        }/// <summary>
        /// The OnAddProduct
        /// </summary>
        private async void OnAddProduct()
        {
            await DialogHost.Show(new AddProductView());
        }

        /// <summary>
        /// The OnBatchAddStock
        /// </summary>
        private async void OnBatchAddStock()
        {
            ServiceLocator.Current.GetInstance<AddStockViewModel>();
            MessengerInstance.Send(new ListOfProductsMessage() { Products = ListOfProducts });
            await DialogHost.Show(new AddStockView(), "RootDialog");
        }

        /// <summary>
        /// The OnBatchDelete
        /// </summary>
        private async void OnBatchDelete()
        {
            await DialogHost.Show(new MessageView(), "RootDialog", delegate (object sender, DialogClosingEventArgs args)
            {
                if (Equals(args.Parameter, false)) return;

                if (Equals(args.Parameter, true))
                {
                    var list = ProductList.Where(c => c.IsSelected).ToList();
                    _context.Products.RemoveRange(list);
                    _context.SaveChanges();
                    DoRefresh(null);
                }
            });
        }

        /// <summary>
        /// The OnClose
        /// </summary>
        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
        }

        /// <summary>
        /// The OnDelete
        /// </summary>
        private async void OnDelete()
        {
            await DialogHost.Show(new MessageView(), "RootDialog", delegate (object sender, DialogClosingEventArgs args)
            {
                if (Equals(args.Parameter, false)) return;

                if (Equals(args.Parameter, true))
                {
                    _context.Products.Remove(SelectedProduct);
                    _context.SaveChanges();
                    DoRefresh(null);
                }
            });
        }

        /// <summary>
        /// The OnEdit
        /// </summary>
        private async void OnEdit()
        {
            ServiceLocator.Current.GetInstance<EditProductViewModel>();
            MessengerInstance.Send(new SelectedProductMessage() { SelectedProduct = SelectedProduct });
            await DialogHost.Show(new EditProductView());
        }

        /// <summary>
        /// The OnView
        /// </summary>
        private async void OnView()
        {
            await DialogHost.Show(new ProductView() { DataContext = this });
        }
    }
}
