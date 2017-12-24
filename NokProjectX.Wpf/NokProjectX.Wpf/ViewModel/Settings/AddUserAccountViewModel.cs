using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
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
using NokProjectX.Wpf.ViewModel.Settings;
using Microsoft.Win32;


namespace NokProjectX.Wpf.ViewModel.Settings
{
    public class AddUserAccountViewModel : ValidatableViewModelBase
    {

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
        private string _username;


        /// <summary>
        /// Defines the _productName
        /// </summary>
        private string _password;

        /// <summary>
        /// Defines the _selectedType
        /// </summary>

        private string _confirmpassword;

        private bool _isOpen;


        /// <summary>
        /// Initializes a new instance of the <see cref="AddUserAccountViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public AddUserAccountViewModel(YumiContext context)
        {
            _context = context;
            LoadCommands();

            ConfigureValidationRules();
        }

    

        /// <summary>
        /// Gets or sets the AddCommand
        /// </summary>
        public RelayCommand AddCommand { get; set; }

        /// <summary>
        /// Gets or sets the CloseCommand
        /// </summary>
        public RelayCommand CloseCommand { get; set; }

     
        

        public bool IsOpen
        {
            get { return _isOpen; }
            set { Set(ref _isOpen, value); }
        }

        private void LoadCommands()
        {
            ViewCommand = new RelayCommand(OnView);
            AddCommand = new RelayCommand(OnAdd);
            CloseCommand = new RelayCommand(OnClose);
        }

        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
            Clear();
            IsOpen = false;
            Validator.Reset();

        }

        private async void OnAdd()
        {
            await ValidateAsync();
            if (HasErrors)
            {
                return;
            }

            UserAccount newUserAccount = new UserAccount()
            {

                Name = LoginName,
                Username = LoginUsername,
                Password = LoginPassword,


            };
            _context.Users.Add(newUserAccount);
            _context.SaveChanges();
            MessengerInstance.Send(new RefreshMessage());

            OnClose();
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


        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public string LoginUsername
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        /// <summary>
        /// Gets or sets the UserAccountName
        /// </summary>
        public string LoginName
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public string LoginPassword
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        public string ConfirmPassword
        {
            get { return _confirmpassword; }
            set { Set(ref _confirmpassword, value); }
        }



        /// <summary>
        /// Gets or sets the ViewCommand
        /// </summary>
        public RelayCommand ViewCommand { get; set; }

        public string Username
        {
            get => _username;
            set => _username = value;
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
            LoginName = null;
            LoginUsername = null;
            LoginPassword = null;

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
            
            Validator.AddRule(nameof(LoginUsername),
                () =>
                {
                    var count = _context.Users.Count(c => c.Username.ToLower().Equals(LoginUsername.Trim().ToLower()));
                    var result = count == 0;
                    return RuleResult.Assert(result,
                        $"UserAccount already exists");
                });

            Validator.AddRequiredRule(() => LoginName, "Name is required");


            Validator.AddRequiredRule(() => LoginUsername, "Username is required");

            Validator.AddRequiredRule(() => LoginPassword, "Password is required");

            Validator.AddRequiredRule(() => ConfirmPassword, "Please Confirm Password");



            
        }
        private async void Validate()
        {
            await ValidateAsync();
        }
    }
}
