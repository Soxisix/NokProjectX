using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using MvvmValidation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;

namespace NokProjectX.Wpf.ViewModel.Settings
{
    public class AddCustomerViewModel : ValidatableViewModelBase{
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the _description
        /// </summary>
        private string _name;

        /// <summary>
        /// Defines the _price
        /// </summary>
        private string _mobile;


        /// <summary>
        /// Defines the _productName
        /// </summary>
        private string _address;

        /// <summary>
        /// Defines the _selectedType
        /// </summary>

        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserAccountViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public AddCustomerViewModel(YumiContext context)
        {
            _context = context;
            LoadCommands();

            ConfigureValidationRules();
        }


        /// <summary>
        /// Gets or sets the AddCommand
        /// </summary>
        public RelayCommand AddCommand { get; set; }/// <summary>
        /// Gets or sets the CloseCommand
        /// </summary>
        public RelayCommand CloseCommand { get; set; }


   
        private void LoadCommands()
        {
         
            AddCommand = new RelayCommand(OnAdd);
            CloseCommand = new RelayCommand(OnClose);
        }

        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
            Clear();   
            Validator.Reset();
        }

        private async void OnAdd()
        {
            await ValidateAsync();
            if (HasErrors)
            {
                return;
            }
     
            Customer newCustomer = new Customer()
            {
                Name = CustomerName,
                Mobile = CustomerMobile,
               Address = CustomerAddress,
            };
            _context.Customers.Add(newCustomer);
            _context.SaveChanges();
            MessengerInstance.Send(new RefreshMessage());

            OnClose();
        }

   

        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public string CustomerName
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        /// <summary>
        /// Gets or sets the UserAccountName
        /// </summary>
        public string CustomerMobile
        {
            get { return _mobile; }
            set { Set(ref _mobile, value); }
        }

        public string CustomerAddress
        {
            get { return _address; }
            set { Set(ref _address, value); }
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
        /// The Clear
        /// </summary>
        internal void Clear()
        {
            CustomerName = null;
            CustomerMobile = null;
            CustomerAddress = null;
          
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

            Validator.AddRule(nameof(CustomerName),
                () =>
                {
                    var count = _context.Customers.Count(c => c.Name.ToLower().Equals(CustomerName.Trim().ToLower()));
                    var result = count == 0;
                    return RuleResult.Assert(result,
                        $"Customer already exists");
                });

            Validator.AddRequiredRule(() => CustomerName, "Name is Required");

            Validator.AddRequiredRule(() => CustomerMobile, "Mobile Number is Required");

            Validator.AddRequiredRule(() => CustomerAddress, "Address is Required");

           
        }

        private async void Validate()
        {
            await ValidateAsync();
        }
    }


}

