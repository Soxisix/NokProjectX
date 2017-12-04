﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.ServiceLocation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;
using NokProjectX.Wpf.Views.Common;
using NokProjectX.Wpf.Views.Inventory;

namespace NokProjectX.Wpf.ViewModel.Inventory
{
    
    public class InventoryViewModel : ViewModelBase
    {
        private readonly YumiContext _context;
        private List<Product> OriginalProductList;

        public InventoryViewModel(YumiContext context)
        {
            _context = context;
            MessengerInstance.Register<RefreshMessage>(this, DoRefresh);
            LoadData();

            LoadCommands();
        }

        private void LoadData()
        {
            OriginalProductList = _context.Products.ToList();
            ProductList = OriginalProductList;
            TotalCount = ProductList.Count;
        }

        private void DoRefresh(RefreshMessage obj)
        {
            LoadData();
        }

        private void LoadCommands()
        {
            AddProductCommand = new RelayCommand(OnAddProduct);
            EditProductCommand = new RelayCommand(OnEdit);
            DeleteProductCommand = new RelayCommand(OnDelete);
        }

        public RelayCommand DeleteProductCommand { get; set; }

        private async void OnDelete()
        {
            await DialogHost.Show(new MessageView());
            _context.Products.Remove(SelectedProduct);
            _context.SaveChanges();
            DoRefresh(null);
        }

        public RelayCommand EditProductCommand { get; set; }

        private async void OnEdit()
        {
            ServiceLocator.Current.GetInstance<EditProductViewModel>();
            MessengerInstance.Send(new SelectedProductMessage() { SelectedProduct = SelectedProduct });
            await DialogHost.Show(new EditProductView());
            
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

        private Product _selectedProduct;
        public Product SelectedProduct { get { return _selectedProduct; } set { Set(ref _selectedProduct,value); } }

        private int _totalCount;
        private string _searchText;

        public int TotalCount
        {
            get { return _totalCount; }
            set
            {
                Set(ref _totalCount, value);
            }
        }

        public string SearchText
        {
            get
            {
                return _searchText;
            }
            set
            {
                Set(ref _searchText, value);
                if (String.IsNullOrEmpty(SearchText))
                {
                    ProductList = OriginalProductList;
                }
                else
                {
                    ProductList = OriginalProductList.Where(c => c.Name.ToLower().Contains(SearchText.Trim().ToLower()) ||
                                                                 SearchText.Trim().Contains(c.ProductCode.ToString()) ||
                                                                 c.Description.ToLower().Contains(SearchText.Trim().ToLower()))
                        .ToList();
                }
            }
        }

    }
}