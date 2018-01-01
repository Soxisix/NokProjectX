using System;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.ServiceLocation;
using NokProjectX.Wpf.Common;
using NokProjectX.Wpf.ViewModel.Reports;
using NokProjectX.Wpf.Views.Customer;

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
        private double _payment;
        private double _change;
        private Customer _selectedCustomer;
        private string _searchCustomer;
        private List<Customer> _originalCustomers;
        private List<Customer> _customers;
        private string _searchProduct;
        private Invoice _newInvoice;
        private bool _isCash
            ;

        private object _paymentMode;

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
        }

        public string SearchProduct
        {
            get { return _searchProduct; }
            set
            {
                Set(ref _searchProduct, value);
                if (String.IsNullOrEmpty(SearchProduct))
                {
                    Products = _originalProducts;
                }
                else
                {
                    Products = _originalProducts.Where(c =>
                            c.Name.ToLower().Contains(SearchProduct.Trim().ToLower()) ||
                            SearchProduct.Trim().Contains(c.ProductCode.ToString()) ||
                            c.Description.ToLower().Contains(SearchProduct.Trim().ToLower()))
                        .ToList();
                    if (Products.Count < 1)
                    {
                        Price = 0.0d;
                    }
                }
            }
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
            set
            {
                Set(ref _invoiceList, value);
                ConfirmCommand.RaiseCanExecuteChanged();
            }
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
                RaisePropertyChanged(() => AddInvoiceCommand);
            }
        }

        public bool IsPieces
        {
            get { return _isPieces; }
            set { Set(ref _isPieces, value); }
        }

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
                    if (value.Type.Name != null && (value.Type.Name == "pcs" || value.Type.Name == "pieces" || value.Type.Name == "pc"))
                    {
                        IsPieces = false;
                    }
                    else
                    {
                        IsPieces = true;
                    }
                }
                Validator.Reset();
                AddInvoiceCommand.RaiseCanExecuteChanged();
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
            set
            {
                Set(ref _total, value);
                if (Payment == 0.0d)
                {
                    Change = 0.0d;
                    return;
                }
                Change = Payment - Total;
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public double Payment
        {
            get { return _payment; }
            set
            {
                Set(ref _payment, value);
                if (Payment == 0.0d)
                {
                    Change = 0.0d;
                    return;
                }
                Change = Payment - Total;
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public double Change
        {
            get { return _change; }
            set { Set(ref _change, value); }
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
            Validator.AddAsyncRule(() => Quantity,
                async () =>
                {
                    if (SelectedProduct.Type.Name == "ft")
                    {
                        if (SelectedProduct.Stock >= (Size1 * Size2) * Quantity)
                        {
                            return RuleResult.Valid();
                        }
                        else
                        {
                            return RuleResult.Invalid("Out of stock!");
                        }
                    }
                    else
                    {
                        if (SelectedProduct.Stock >=  Quantity)
                        {
                            return RuleResult.Valid();
                        }
                        else
                        {
                            return RuleResult.Invalid("Out of stock!");
                        }
                    }
                });

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
            ResetCommand = new RelayCommand(OnReset);
            AddInvoiceCommand = new RelayCommand(OnAddInvoice, () => SelectedProduct != null);
            RemoveInvoiceCommand = new RelayCommand(OnRemoveInvoice);
            ClearPaymentCommand = new RelayCommand(OnClearPayment);
            NewCustomerCommand = new RelayCommand(OnNewCustomer);
            AddCustomerCommand = new RelayCommand(OnAddCustomer);
            CloseCommand = new RelayCommand(OnClose);
            ConfirmCommand = new RelayCommand(OnConfirm, () =>
            {
                
                if (InvoiceList != null && SelectedCustomer != null && PaymentMethod())
                {
                    return true;
                }
                return false;
            });
        }

        public RelayCommand ResetCommand { get; set; }

        private void OnReset()
        {
            foreach (var invoice in InvoiceList)
            {
                _context.Entry(invoice.Product).ReloadAsync();
            }
            ClearTransaction();
            ClearFields();
            RaisePropertyChanged(() => SelectedProduct);
            RaisePropertyChanged(() => Products);
        }

        bool PaymentMethod()
        {
            if (IsCash)
            {
                if (Payment >= Total)
                {
                    return true;
                }
                return false;
            }
            else
            {
                return true;
            }

        }

        public RelayCommand ConfirmCommand { get; set; }

        private void OnConfirm()
        {
            var payment = 0.0d;
            if (IsCash)
            {
                payment = Total;
            }
            else
            {
                payment = 0.0d;
            }
            var transaction = new Transaction()
            {
                Payment = payment,
                TotalPrice = Total,
                Date = DateTime.Now,
                Invoice = NewInvoice,
                Customer = SelectedCustomer
            };
            foreach (var invoice in InvoiceList)
            {
                invoice.Customer = SelectedCustomer;
                invoice.Date = DateTime.Now;
            }
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
            ClearTransaction();
            ClearFields();
            MessageBox.Show("Transaction Successful");
            ServiceLocator.Current.GetAllInstances<ReportViewModel>();
            MessengerInstance.Send(new RefreshMessage());
        }

        void ClearTransaction()
        {
            InvoiceList = new ObservableCollection<Invoice>();
            SelectedCustomer = null;
            SearchProduct = null;
            Payment = 0.0d;}
        public RelayCommand CloseCommand { get; set; }

        private void OnClose()
        {
            Validator.Reset();
            ClearNewCustomer();
            DialogHost.CloseDialogCommand.Execute(this, null);
        }

        void ClearNewCustomer()
        {
            CustomerName = null;
            CustomerMobile = null;
            CustomerAddress = null;
        }

        public RelayCommand AddCustomerCommand { get; set; }

        private async void OnAddCustomer()
        {
            await ValidateAsync();
            if (HasErrors)
            {
                return;
            }
            var newCustomer = new Customer()
            {
                Name = CustomerName,
                Mobile = CustomerMobile,
                Address = CustomerAddress
            };
            _originalCustomers.Add(newCustomer);

            _context.Customers.Add(newCustomer);
            _context.SaveChanges();
            LoadData();
            OnClose();
        }

        private void ValidateCustomer()
        {
            Validator.AddRequiredRule(() => CustomerName, "Customer Name is Required!");
            Validator.AddRequiredRule(() => CustomerMobile, "Mobile Number is Required!");
        }

        public RelayCommand NewCustomerCommand { get; set; }

        private async void OnNewCustomer()
        {
            Validator.RemoveAllRules();
            ValidateCustomer();
            await DialogHost.Show(new NewCustomerView() {DataContext = this}, "RootDialog");
        }

        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerAddress { get; set; }

        public List<Customer> Customers
        {
            get { return _customers; }
            set { Set(ref _customers, value); }
        }

        public string SearchCustomer
        {
            get { return _searchCustomer; }
            set
            {
                Set(ref _searchCustomer, value);

                if (String.IsNullOrEmpty(SearchCustomer))
                {
                    Customers = _originalCustomers;
                }
                else
                {
                    Customers = _originalCustomers
                        .Where(c => c.Name.ToLower().Contains(SearchCustomer.ToLower().Trim())).ToList();
                }
            }
        }

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                Set(ref _selectedCustomer, value);
                ConfirmCommand.RaiseCanExecuteChanged();
                
            }
        }

        public RelayCommand ClearPaymentCommand { get; set; }

        private void OnClearPayment()
        {
            Payment = 0.0d;
        }

        /// <summary>
        /// The LoadData
        /// </summary>
        private void LoadData()
        {
            _originalProducts = _context.Products.ToList();
            Products = _originalProducts;
            _originalCustomers = _context.Customers.ToList();
            Customers = _originalCustomers;
        }

        /// <summary>
        /// The OnAddInvoice
        /// </summary>
        private async void OnAddInvoice()
        {
            if (SelectedProduct == null)
            {
                return;
            }
            Validator.RemoveAllRules();
            ConfigureValidationRules();
            await ValidateAsync();
            if (HasErrors) return;
            double price = 0.0d;
            string size;
            double unit = 0.0d;
            if (SelectedProduct.Type.Name == "pcs" || SelectedProduct.Type.Name == "pieces" ||
                SelectedProduct.Type.Name == "pc")
            {
                price = Price * Quantity.GetValueOrDefault();
                unit = Price;
                size = null;
                SelectedProduct.Stock -= Quantity.GetValueOrDefault();
            }
            else
            {
                unit = (Quantity.GetValueOrDefault() *
                        (Size1.GetValueOrDefault() * Size2.GetValueOrDefault()));
                price = Price * unit;
                SelectedProduct.Stock -= (Quantity.GetValueOrDefault() *
                                          (Size1.GetValueOrDefault() * Size2.GetValueOrDefault()));
                size = $"{Size1} x {Size2}";
            }
            RaisePropertyChanged(() => SelectedProduct);
            RaisePropertyChanged(() => Products);
            var code = _context.Invoices.Select(c => c.InvoiceCode).OrderByDescending(c => c).FirstOrDefault();
            int finalNumber = 0;
            if (code > 0)
            {
                finalNumber = code + 1;
            }
            else
            {
                finalNumber = 1000001;
            }
            NewInvoice = new Invoice()
            {
                InvoiceCode = finalNumber,
                Product = SelectedProduct,
                Unit = unit,
                Price = price,
                Length = Size1.GetValueOrDefault(),
                Width = Size2.GetValueOrDefault(),
                Quantity = Quantity.GetValueOrDefault(),
                Size = $"{Size1} x {Size2}",
                Description = Description
            };

            InvoiceList.Add(NewInvoice);
            CalculateTotal();
            Total = Total;
            ClearFields();
        }

        public Invoice NewInvoice
        {
            get { return _newInvoice; }
            set { Set(ref _newInvoice, value); }
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

                if (SelectedInvoice.Product.Type.Name == "ft")
                {
                    SelectedInvoice.Product.Stock +=
                        (SelectedInvoice.Length * SelectedInvoice.Width) * SelectedInvoice.Quantity;
                }
                else
                {
                    SelectedInvoice.Product.Stock += SelectedInvoice.Quantity;
                }
                InvoiceList.Remove(SelectedInvoice);
                RaisePropertyChanged(() => Products);
                RaisePropertyChanged(() => SelectedProduct);
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

        public bool IsCash
        {
            get
            {
                return _isCash;
            }
            set
            {
                Set(ref _isCash, value);
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public object PaymentMode
        {
            get
            {
                return _paymentMode;
            }
            set
            {
                Set(ref _paymentMode, value);
                if (value != null)
                {
                    var a = value as ListBoxItem;
                    if (a != null && a.Name == "Cash")
                    {
                        IsCash = true;
                    }
                    if (a != null && a.Name == "Credit")
                    {
                        IsCash = false;
                    }
                }
            }
        }
    }
}
