using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using MvvmValidation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;
using Type = NokProjectX.Wpf.Entities.Type;

namespace NokProjectX.Wpf.ViewModel.Inventory
{
    public class AddProductViewModel : ValidatableViewModelBase
    {
        private readonly YumiContext _context;

        public AddProductViewModel(YumiContext context)
        {
            _context = context;
            LoadCommands();
            Types = _context.Types.ToList();
            ConfigureValidationRules();

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
            Validator.Reset();
            DialogHost.CloseDialogCommand.Execute(this, null);

        }

        private async void OnAdd()
        {
            await ValidateAsync();
            if (HasErrors)
            {
                return;
            }
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
                Type = SelectedType,
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


        private async void Validate()
        {
            await ValidateAsync();
        }

        public async Task ValidateAsync()
        {
            await Validator.ValidateAllAsync();
            
        }

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
            Validator.AddRequiredRule(() => ProductName, "Product Name is required");
            Validator.AddAsyncRule(nameof(ProductName),
                validateAction: async () =>
                {
                    var count = _context.Products.Count(c => c.Name.ToLower().Equals(ProductName.Trim().ToLower()));
                    var result = count == 0;
                    return RuleResult.Assert(result,
                        $"Product already exists");
                });

            Validator.AddRequiredRule(() => Description, "Description is required");

            Validator.AddRequiredRule(() => SelectedType, "Type is required");

            Validator.AddRequiredRule(() => Stock, "Stock is required");

            Validator.AddAsyncRule(nameof(Stock),
                validateAction: async () =>
                {
                    int num;
                    var result = int.TryParse(Stock, out num);
                    return RuleResult.Assert(result,
                        $"Stock must be a number.");
                });

            Validator.AddRequiredRule(() => Price, "Price is required");

            Validator.AddAsyncRule(nameof(Price),
                validateAction: async () =>
                {
                    double num;
                    var result = double.TryParse(Price, out num);
                    return RuleResult.Assert(result,
                        $"Price must be a number.");
                });


        }

    }
}