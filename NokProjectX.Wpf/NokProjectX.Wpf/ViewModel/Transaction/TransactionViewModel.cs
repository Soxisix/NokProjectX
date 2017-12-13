using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using GalaSoft.MvvmLight.Command;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;

namespace NokProjectX.Wpf.ViewModel.Transaction
{
    public class TransactionViewModel : ValidatableViewModelBase
    {
        private readonly YumiContext _context;

        public TransactionViewModel(YumiContext context)
        {
            _context = context;
            LoadData();
            MessengerInstance.Register<RefreshMessage>(this, OnRefresh);
            LoadCommand();
            InvoiceList = new ObservableCollection<Invoice>();
        }

        private void LoadCommand()
        {
            AddInvoiceCommand = new RelayCommand(OnAddInvoice);
            RemoveInvoiceCommand = new RelayCommand(OnRemoveInvoice);
        }

        public RelayCommand RemoveInvoiceCommand { get; set; }

        private void OnRemoveInvoice()
        {
            if (SelectedInvoice != null)
            {
                InvoiceList.Remove(SelectedInvoice);
            }
        }

        public RelayCommand AddInvoiceCommand { get; set; }

        private void OnAddInvoice()
        {
            var newInvoice = new Invoice()
            {
                Product = SelectedProduct,
                Price = Price,
                Quantity = Quantity.GetValueOrDefault()
                
            };
            InvoiceList.Add(newInvoice);
        }

        
        public int? Quantity { get { return _quantity; } set { Set(ref _quantity, value); } }


        public double Price
        {
            get
            {
                return _price;
                
            }
            set
            {
                Set(ref _price, value);
            }
        }

        private void OnRefresh(RefreshMessage obj)
        {
            LoadData();
        }

        private void LoadData()
        {
            _originalProducts = _context.Products.ToList();
            Products = _originalProducts;
        }

        private List<Product> _originalProducts;
        private List<Product> _products;
        public List<Product> Products { get { return _products; } set { Set(ref _products, value); } }


        private Product _selectedProduct;
        private ObservableCollection<Invoice> _invoiceList;
        private double _price;
        private int? _quantity;
        private Invoice _selectedInvoice;

        public Product SelectedProduct
        {
            get
            {
                return _selectedProduct;
            }
            set
            {
                Set(ref _selectedProduct, value);
                if (value != null)
                {
                    Price = value.Price;
                }
                
            }
        }

        public ObservableCollection<Invoice> InvoiceList
        {
            get { return _invoiceList; }
            set { Set(ref _invoiceList, value); }
        }

        public Invoice SelectedInvoice { get { return _selectedInvoice; } set { Set(ref _selectedInvoice, value); } }    

    }
}