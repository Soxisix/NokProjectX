using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;
using Type = NokProjectX.Wpf.Entities.Type;

namespace NokProjectX.Wpf.ViewModel.Inventory
{
    public class AddProductViewModel : ViewModelBase
    {
        private readonly YumiContext _context;

        public AddProductViewModel(YumiContext context)
        {
            _context = context;
            LoadCommands();
            Types = _context.Types.ToList();
            
        }

        public RelayCommand CloseCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand ViewCommand { get; set; }
        private void LoadCommands()
        {
            ViewCommand = new RelayCommand(OnView);
            AddCommand = new RelayCommand(OnAdd);
            CloseCommand = new RelayCommand(OnClose);
        }

        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
        }

        private async void OnAdd()
        {
            int newCode = 0;
            var result =_context.Products.Select(c => c.ProductCode).OrderByDescending(c => c).FirstOrDefault();
            if (result != null)
            {
                newCode = result + 1;
            }
            Product newProduct = new Product()
            {
                ProductCode = newCode,
                Name = ProductName,
                Description = Description,
                Type = Type,
                Stock = Int32.Parse(Stock),
                Price = Double.Parse(Price)
            };
            _context.Products.Add(newProduct);
            _context.SaveChanges();
            MessengerInstance.Send(new RefreshMessage());
            //Clear();
            OnClose();
        }

        void Clear()
        {
            ProductName = null;
            Description = null;
            SelectedType = null;
            Stock = null;
            Price = null;
        }

        private void OnView()
        {
            if (IsOpen)
            {
                IsOpen = false;
                return;
            }
            IsOpen = true;
        }

        private bool _isOpen;

        public bool IsOpen
        {
            get { return _isOpen; }
            set { Set(ref _isOpen, value); }
        }

        private string _productName;
        public string ProductName { get { return _productName; } set { Set(ref _productName, value); } }
        private string _description;
        public string Description { get { return _description; } set { Set(ref _description, value); } }
        private string _type;
        public string Type { get { return _type; } set { Set(ref _type, value); } }
        private string _stock;
        public string Stock { get { return _stock; } set { Set(ref _stock, value); } }
        private string _price;
        public string Price { get { return _price; } set { Set(ref _price, value); } }

        private List<Type> _types;
        public List<Type> Types { get { return _types; }set { Set(ref _types,value); } }
        private Type _selectedType;
        public Type SelectedType { get { return _selectedType; } set { Set(ref _selectedType, value); } }

    }
}