namespace NokProjectX.Wpf.ViewModel.Inventory
{
    using GalaSoft.MvvmLight.Command;
    using MaterialDesignThemes.Wpf;
    using MvvmValidation;
    using NokProjectX.Wpf.Common.Messages;
    using NokProjectX.Wpf.Common.Validator;
    using NokProjectX.Wpf.Context;
    using NokProjectX.Wpf.Entities;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AddStockViewModel" />
    /// </summary>
    public class AddStockViewModel : ValidatableViewModelBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the _listOfProducts
        /// </summary>
        private List<Product> _listOfProducts;

        /// <summary>
        /// Defines the _stock
        /// </summary>
        private int? _stock;

        /// <summary>
        /// Initializes a new instance of the <see cref="AddStockViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public AddStockViewModel(YumiContext context)
        {
            _context = context;
            MessengerInstance.Register<ListOfProductsMessage>(this, OnListRecieve);
            //            var t = _context.Products.GetType();
            LoadCommands();
        }

        /// <summary>
        /// Gets or sets the CloseCommand
        /// </summary>
        public RelayCommand CloseCommand { get; set; }

        /// <summary>
        /// Gets or sets the EditCommand
        /// </summary>
        public RelayCommand EditCommand { get; set; }

        /// <summary>
        /// Gets or sets the Stock
        /// </summary>
        public int? Stock
        {
            get { return _stock; }
            set { Set(ref _stock, value); }
        }

        //        private async void Validate()
        //        {
        //            await ValidateAsync();
        //        }
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
            Validator.AddRequiredRule(() => Stock, "Stock is required");
        }

        /// <summary>
        /// The LoadCommands
        /// </summary>
        private void LoadCommands()
        {
            EditCommand = new RelayCommand(OnEdit);
            CloseCommand = new RelayCommand(OnClose);
            ConfigureValidationRules();
        }

        /// <summary>
        /// The OnClose
        /// </summary>
        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
            Stock = null;
            Validator.Reset();
        }

        /// <summary>
        /// The OnEdit
        /// </summary>
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

        /// <summary>
        /// The OnListRecieve
        /// </summary>
        /// <param name="obj">The <see cref="ListOfProductsMessage"/></param>
        private void OnListRecieve(ListOfProductsMessage obj)
        {
            if (obj == null)
            {
                return;
            }
            _listOfProducts = obj.Products;
        }
    }
}
