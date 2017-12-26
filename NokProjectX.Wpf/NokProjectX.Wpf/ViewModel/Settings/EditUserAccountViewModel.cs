using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using MvvmValidation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;
using NokProjectX.Wpf.ViewModel.Inventory;
using System;
using System.ComponentModel.DataAnnotations;


namespace NokProjectX.Wpf.ViewModel.Settings
{
    public class EditUserAccountViewModel : ValidatableViewModelBase
    {
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the _currentUserAccount
        /// </summary>
        private UserAccount _currentUserAccount;

        /// <summary>
        /// Defines the _description
        /// </summary>
        private int _id;

        /// <summary>
        /// Defines the _isOpen
        /// </summary>
        private bool _isOpen;

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
        private string _username;

        private string _password;

        private string _confirmpassword;

   
      
        /// <summary>
        /// Initializes a new instance of the <see cref="EditUserAccountViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public EditUserAccountViewModel(YumiContext context)
        {
            _context = context;
            LoadCommands();
          
            ConfigureValidationRules();
            MessengerInstance.Register<SelectedUserMessage>(this, OnUserAccountRecieve);
        }

        /// <summary>
        /// Gets or sets the CloseCommand
        /// </summary>
        public RelayCommand CloseCommand { get; set; }
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public int Id
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
        public string UserAccountName
        {
            get { return _name; }
            set { Set(ref _name, value); }
        }

        public string UserAccountUsername
        {
            get { return _username; }
            set { Set(ref _username, value); }
        }

        public string UserAccountPassword
        {
            get { return _password; }
            set { Set(ref _password, value); }
        }

        public string UserAccountConfirmPassword
        {
            get { return _confirmpassword; }
            set { Set(ref _confirmpassword, value); }
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
            UserAccountName = null;
            UserAccountUsername = null;
            UserAccountPassword = null;
            UserAccountConfirmPassword = null;
          
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
            Validator.AddRequiredRule(() => UserAccountName, " Name is Required");
            //            Validator.AddAsyncRule(nameof(UserAccountName),
            //                validateAction: async () =>
            //                {
            //                    var count = _context.UserAccounts.Count(c => c.Name.ToLower().Equals(UserAccountName.Trim().ToLower()));
            //                    var result = count == 0;
            //                    return RuleResult.Assert(result,
            //                        $"UserAccount already exists");
            //                });

            Validator.AddRequiredRule(() => UserAccountUsername, "Username is Required");

            Validator.AddRequiredRule(() => UserAccountPassword, "Password is Required");

            Validator.AddRequiredRule(() => UserAccountConfirmPassword, "Please Confirm Password");

            //            Validator.AddAsyncRule(nameof(Stock),
            //                validateAction: async () =>
            //                {
            //                    int num;
            //                    var result = int.TryParse(Stock, out num);
            //                    return RuleResult.Assert(result,
            //                        $"Stock must be a number.");
            //                });

          
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
            var UserAccount = _context.Users.FirstOrDefault(c => c.Id.Equals(_currentUserAccount.Id));
            if (UserAccount != null)
            {
                UserAccount.Name = UserAccountName;
                UserAccount.Username= UserAccountUsername;
                UserAccount.Password = UserAccountPassword;
                UserAccount.ConfirmPassword = UserAccountConfirmPassword;
            }
            _context.SaveChanges();
            MessengerInstance.Send(new RefreshMessage());

            OnClose();
        }

        /// <summary>
        /// The OnUserAccountRecieve
        /// </summary>
        /// <param name="obj">The <see cref="SelectedUserMessage"/></param>
        private void OnUserAccountRecieve(SelectedUserMessage obj)
        {_currentUserAccount = obj.SelectedUser;
            if (obj.SelectedUser == null)
            {
                return;
            }
            UserAccountName = _currentUserAccount.Name;
            UserAccountUsername = _currentUserAccount.Username;
            UserAccountPassword= _currentUserAccount.Password;
            UserAccountConfirmPassword = _currentUserAccount.ConfirmPassword;
         
        }

       
    
    }
}
