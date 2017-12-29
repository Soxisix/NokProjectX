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
    public class EditCustomerViewModel : ValidatableViewModelBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the _currentUserAccount
        /// </summary>
        private Customer _currentCustomer;

        /// <summary>
        /// Defines the _description
        /// </summary>
        private int _id;

        /// <summary>
        /// Defines the _isOpen
        /// </summary>
        private bool _isOpen;



        /// <summary>
        /// Defines the _price
        /// </summary>
        private string _mobile;


        /// <summary>
        /// Defines the _productName
        /// </summary>
        private string _address;


        /// <summary>
        /// Defines the _picture
        /// </summary>


        /// <summary>
        /// Defines the _price
        /// </summary>
        private string _name;

        /// <summary>
        /// Defines the _UserAccountName
        /// </summary>
       
   
      
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCustomerViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public EditCustomerViewModel(YumiContext context)
        {
            _context = context;
            LoadCommands();
          
            ConfigureValidationRules();
            MessengerInstance.Register<SelectedCustomerMessage>(this, OnCustomerReceive);
        }

        /// <summary>
        /// Gets or sets the CloseCommand
        /// </summary>
        public RelayCommand CloseCommand { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public int CustomerId
        {
            get { return _id; }
            set { Set(ref _id, value); }
        }

        /// <summary>
        /// Gets or sets the EditCommand
        /// </summary>
        public RelayCommand EditCommand { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsOpen
        /// </summary>
        public bool IsOpen
        {
            get { return _isOpen; }
            set { Set(ref _isOpen, value); }
        }


        /// <summary>
        /// Gets or sets the UserAccountName
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
        /// Gets or sets the SelectedType
        /// </summary>



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
            Validator.AddRequiredRule(() => CustomerName, "Name is Required");

            Validator.AddRequiredRule(() => CustomerMobile, "Mobile Number is Required");

            Validator.AddRequiredRule(() => CustomerAddress, "Address is Required");


        }

        /// <summary>
        /// The LoadCommands
        /// </summary>
        private void LoadCommands()
        {
           
            EditCommand = new RelayCommand(OnEdit);
            CloseCommand = new RelayCommand(OnClose);
         
        }

        /// <summary>
        /// The OnClose
        /// </summary>
        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
            Clear();
            IsOpen = false;
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
            //            int newCode = 0;
            //            var result = _context.UserAccounts.Select(c => c.UserAccountCode).OrderByDescending(c => c).FirstOrDefault();
            //            if (result != null)
            //            {
            //                newCode = result + 1;
            //            }
            //            UserAccount newUserAccount = new UserAccount()
            //            {
            //                Name = UserAccountName,
            //                Description = Description,
            //                Type = SelectedType,
            //                Stock = Int32.Parse(Stock),
            //                Price = Double.Parse(Price)
            //            };
            //            _context.UserAccounts.AddOrUpdate(newUserAccount);
            var Customer = _context.Customers.FirstOrDefault(c => c.Id.Equals(_currentCustomer.Id));

            if (Customer != null)
            {
                Customer.Name = CustomerName;
               Customer.Mobile = CustomerMobile;
               Customer.Address = CustomerAddress;

            }
            _context.SaveChanges();MessengerInstance.Send(new RefreshMessage());

            OnClose();
        }

        /// <summary>
        /// The OnUserAccountRecieve
        /// </summary>
        /// <param name="obj">The <see cref="SelectedCustomerMessage"/></param>
        private void OnCustomerReceive(SelectedCustomerMessage obj)
        {
            if (obj.SelectedCustomer == null)
            {
                return;
            }
            _currentCustomer = obj.SelectedCustomer;
            CustomerId = _currentCustomer.Id;
            CustomerName= _currentCustomer.Name;
            CustomerMobile =_currentCustomer.Mobile;
           CustomerAddress= _currentCustomer.Address;
         
            //            LoginConfirmPassword = _currentUserAccount.ConfirmPassword;;

        }

       
    
    }
    }

