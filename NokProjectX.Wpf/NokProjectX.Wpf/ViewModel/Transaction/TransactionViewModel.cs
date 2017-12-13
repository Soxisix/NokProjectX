using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
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
        public Product SelectedProduct { get { return _selectedProduct; } set { Set(ref _selectedProduct, value); } }    

    }
}