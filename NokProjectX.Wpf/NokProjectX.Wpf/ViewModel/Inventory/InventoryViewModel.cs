using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using GalaSoft.MvvmLight;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;

namespace NokProjectX.Wpf.ViewModel.Inventory
{
    
    public class InventoryViewModel : ViewModelBase
    {
        private readonly YumiContext _context;

        public InventoryViewModel(YumiContext context)
        {
            _context = context;
            
            var i = context.Products.ToList();
            ProductList = i;
            TotalCount = i.Count;
        }

        private List<Product> _productList;

        public List<Product> ProductList
        {
            get { return _productList; }
            set { Set(ref _productList, value); }
        }

        private int _totalCount;

        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                Set(ref _totalCount, value);
            }
        }

    }
}