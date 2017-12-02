using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignThemes.Wpf;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;
using NokProjectX.Wpf.Views.Inventory;

namespace NokProjectX.Wpf.ViewModel.Inventory
{
    
    public class InventoryViewModel : ViewModelBase
    {
        private readonly YumiContext _context;

        public InventoryViewModel(YumiContext context)
        {
            _context = context;
            MessengerInstance.Register<RefreshMessage>(this, DoRefresh);
            LoadData();

            LoadCommands();
        }

        private void LoadData()
        {
            var i = _context.Products.ToList();
            ProductList = i;
            TotalCount = i.Count;
        }

        private void DoRefresh(RefreshMessage obj)
        {
            LoadData();
        }

        private void LoadCommands()
        {
            AddProductCommand = new RelayCommand(OnAddProduct);
        }

        public RelayCommand AddProductCommand { get; set; }

        private async void OnAddProduct()
        {
            await DialogHost.Show(new AddProductView());
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