using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using MvvmValidation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;

namespace NokProjectX.Wpf.ViewModel.Inventory
{
    public class AddStockViewModel : ValidatableViewModelBase
    {
        private readonly YumiContext _context;
        private List<Product> _listOfProducts;

        public AddStockViewModel(YumiContext context)
        {
            _context = context;
            MessengerInstance.Register<ListOfProductsMessage>(this, OnListRecieve);
//            var t = _context.Products.GetType();
            LoadCommands();
        }

        public RelayCommand CloseCommand { get; set; }
        public RelayCommand EditCommand { get; set; }

        private void LoadCommands()
        {
            EditCommand = new RelayCommand(OnEdit);
            CloseCommand = new RelayCommand(OnClose);
            ConfigureValidationRules();
        }

        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
            Stock = null;
            Validator.Reset();
        }

        private async void OnEdit()
        {
            await ValidateAsync();
            if (HasErrors)
            {
                return;
            }
            foreach (var product in _listOfProducts)
            {
                product.Stock = product.Stock + Stock.GetValueOrDefault();
            }

            var selectedItems = _context.Products.Where(c => c.IsSelected);
            foreach (var item in selectedItems)
            {
                item.IsSelected = false;
            }

            _context.SaveChanges();

            MessengerInstance.Send(new RefreshMessage());

            OnClose();
        }

        private void OnListRecieve(ListOfProductsMessage obj)
        {
            if (obj == null)
            {
                return;
            }
            _listOfProducts = obj.Products;
        }


        private int? _stock;

        public int? Stock
        {
            get { return _stock; }
            set { Set(ref _stock, value); }
        }

//        private async void Validate()
//        {
//            await ValidateAsync();
//        }

        public async Task ValidateAsync()
        {
            await Validator.ValidateAllAsync();
        }

        private void ConfigureValidationRules()
        {
            Validator.AddRequiredRule(() => Stock, "Stock is required");
        }

        //
        //        public List<string> ProductNames
        //        {
        //            get
        //            {
        //                return typeof(Product).GetProperties()
        //                    .Select(property => property.Name)
        //                    .ToList();
        //            }
        //        }
        //
        //        private string _selectedName;
        //        public string SelectedName { get { return _selectedName; } set { Set(ref _selectedName, value); } }
    }
}