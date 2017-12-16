namespace NokProjectX.Wpf.ViewModel.Transaction
{
    using GalaSoft.MvvmLight.Command;
    using MvvmValidation;
    using NokProjectX.Wpf.Common.Messages;
    using NokProjectX.Wpf.Common.Validator;
    using NokProjectX.Wpf.Context;
    using NokProjectX.Wpf.Entities;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="TransactionViewModel" />
    /// </summary>
    public class TransactionViewModel : ValidatableViewModelBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the _description
        /// </summary>
        private string _description;

        /// <summary>
        /// Defines the _invoiceList
        /// </summary>
        private ObservableCollection<Invoice> _invoiceList;

        /// <summary>
        /// Defines the _originalProducts
        /// </summary>
        private List<Product> _originalProducts;

        /// <summary>
        /// Defines the _price
        /// </summary>
        private double _price;

        /// <summary>
        /// Defines the _products
        /// </summary>
        private List<Product> _products;

        /// <summary>
        /// Defines the _quantity
        /// </summary>
        private int? _quantity;

        /// <summary>
        /// Defines the _selectedInvoice
        /// </summary>
        private Invoice _selectedInvoice;

        /// <summary>
        /// Defines the _selectedProduct
        /// </summary>
        private Product _selectedProduct;

        /// <summary>
        /// Defines the _size1
        /// </summary>
        private double? _size1;

        /// <summary>
        /// Defines the _size2
        /// </summary>
        private double? _size2;

        /// <summary>
        /// Defines the _total
        /// </summary>
        private double _total;

        private bool _isPieces;

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public TransactionViewModel(YumiContext context)
        {
            _context = context;
            LoadData();
            MessengerInstance.Register<RefreshMessage>(this, OnRefresh);
            LoadCommand();
            InvoiceList = new ObservableCollection<Invoice>();
            ConfigureValidationRules();
        }

        /// <summary>
        /// Gets or sets the AddInvoiceCommand
        /// </summary>
        public RelayCommand AddInvoiceCommand { get; set; }

        /// <summary>
        /// Gets or sets the ChangePriceCommand
        /// </summary>
        

        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { Set(ref _description, value); }
        }

        /// <summary>
        /// Gets or sets the InvoiceList
        /// </summary>
        public ObservableCollection<Invoice> InvoiceList
        {
            get { return _invoiceList; }
            set { Set(ref _invoiceList, value); }
        }

        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public double Price
        {
            get { return _price; }
            set { Set(ref _price, value); }
        }

        /// <summary>
        /// Gets or sets the Products
        /// </summary>
        public List<Product> Products
        {
            get { return _products; }
            set { Set(ref _products, value); }
        }

        /// <summary>
        /// Gets or sets the Quantity
        /// </summary>
        public int? Quantity
        {
            get { return _quantity; }
            set { Set(ref _quantity, value); }
        }

        /// <summary>
        /// Gets or sets the RemoveInvoiceCommand
        /// </summary>
        public RelayCommand RemoveInvoiceCommand { get; set; }

        /// <summary>
        /// Gets or sets the SelectedInvoice
        /// </summary>
        public Invoice SelectedInvoice
        {
            get { return _selectedInvoice; }
            set
            {
                Set(ref _selectedInvoice, value);
                Validator.Reset();
            }
        }

        public bool IsPieces { get { return _isPieces; } set { Set(ref _isPieces, value); } }
        /// <summary>
        /// Gets or sets the SelectedProduct
        /// </summary>
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                Set(ref _selectedProduct, value);
                if (value != null)
                {
                    Price = value.Price;
                    ClearFields();
                    if (value.Type.Name == "pcs" || value.Type.Name == "pieces" || value.Type.Name == "pc")
                    {
                        IsPieces = false;
                    }
                    else
                    {
                        IsPieces = true;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets the Size1
        /// </summary>
        public double? Size1
        {
            get { return _size1; }
            set { Set(ref _size1, value); }
        }

        /// <summary>
        /// Gets or sets the Size2
        /// </summary>
        public double? Size2
        {
            get { return _size2; }
            set { Set(ref _size2, value); }
        }

        /// <summary>
        /// Gets or sets the Total
        /// </summary>
        public double Total
        {
            get { return _total; }
            set { Set(ref _total, value); }
        }

        /// <summary>
        /// The ValidateAsync
        /// </summary>
        /// <returns>The <see cref="Task"/></returns>
        public async Task ValidateAsync()
        {
            await Validator.ValidateAllAsync();
        }

        /// <summary>
        /// The ConfigureValidationRules
        /// </summary>
        private void ConfigureValidationRules()
        {
            //            Validator.AddAsyncRule(nameof(LRN),
            //                async () =>
            //                {
            //                    var _context = new MorenoContext();
            //                    var result = await _context.Students.FirstOrDefaultAsync(e => e.LRN == LRN);
            //                    bool isAvailable = result == null;
            //                    return RuleResult.Assert(isAvailable,
            //                        string.Format("LRN {0} is taken. Please choose a different one.", LRN));
            //                });
            Validator.AddRequiredRule(() => Quantity, "Quantity is required");
            
            Validator.AddRequiredRule(() => Description, "Description is required");

            if (IsPieces)
            {
                Validator.AddRequiredRule(() => Size1, "Length is required");
                Validator.AddRequiredRule(() => Size2, "Width is required");
            }
            

        }

        /// <summary>
        /// The LoadCommand
        /// </summary>
        private void LoadCommand()
        {
            AddInvoiceCommand = new RelayCommand(OnAddInvoice);
            RemoveInvoiceCommand = new RelayCommand(OnRemoveInvoice);
            
        }

        /// <summary>
        /// The LoadData
        /// </summary>
        private void LoadData()
        {
            _originalProducts = _context.Products.ToList();
            Products = _originalProducts;
        }

        /// <summary>
        /// The OnAddInvoice
        /// </summary>
        private async void OnAddInvoice()
        {
            await ValidateAsync();
            if (HasErrors)  return;
            
            var newInvoice = new Invoice()
            {
                Product = SelectedProduct,
                Price = Price * (Quantity.GetValueOrDefault() *(Size1.GetValueOrDefault() * Size2.GetValueOrDefault())),
                Quantity = Quantity.GetValueOrDefault(),
                Size = $"{Size1} x {Size2}",
                Description = Description
            };
            InvoiceList.Add(newInvoice);
            CalculateTotal();
            Total = Total;
            ClearFields();
        }


        /// <summary>
        /// The OnRefresh
        /// </summary>
        /// <param name="obj">The <see cref="RefreshMessage"/></param>
        private void OnRefresh(RefreshMessage obj)
        {
            LoadData();
        }

        /// <summary>
        /// The OnRemoveInvoice
        /// </summary>
        private void OnRemoveInvoice()
        {
            if (SelectedInvoice != null)
            {
                InvoiceList.Remove(SelectedInvoice);
                
            }
            CalculateTotal();
        }

        private void CalculateTotal()
        {
            var total = 0.0d;
            foreach (var invoice in InvoiceList)
            {
                total = total + invoice.Price;
            }
            Total = total;
        }

        void ClearFields()
        {
            Quantity = null;
            Size1 = null;
            Size2 = null;
            Description = null;
        }
    }
}
