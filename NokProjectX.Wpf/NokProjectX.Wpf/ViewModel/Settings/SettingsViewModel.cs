using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.ServiceLocation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;
using NokProjectX.Wpf.ViewModel.Settings;
using NokProjectX.Wpf.Views.Settings;
using NokProjectX.Wpf.Views.Common;
using RelayCommand = GalaSoft.MvvmLight.CommandWpf.RelayCommand;

namespace NokProjectX.Wpf.ViewModel.Settings
{
    public class SettingsViewModel : ViewModelBase
    {
        
    
        /// <summary>
        /// Defines the _context
        /// </summary>
        private readonly YumiContext _context;

        /// <summary>
        /// Defines the _useraccountList
        /// </summary>
        private List<UserAccount> _useraccountList;

        /// <summary>
        /// Defines the _searchText
        /// </summary>
        private string _searchText;

        /// <summary>
        /// Defines the _selectedUserAccount
        /// </summary>
        private UserAccount _selectedUserAccount;

        /// <summary>
        /// Defines the _totalCount
        /// </summary>
        private int _totalCount;

        /// <summary>
        /// Defines the OriginalUserAccountList
        /// </summary>
        private List<UserAccount> OriginalUserAccountList;

        /// <summary>
        /// Initializes a new instance of the <see cref="InventoryViewModel"/> class.
        /// </summary>
        /// <param name="context">The <see cref="YumiContext"/></param>
        public SettingsViewModel(YumiContext context)
        {
            _context = context;
            MessengerInstance.Register<RefreshMessage>(this, DoRefresh);
            LoadData();

            LoadCommands();
        }

        /// <summary>
        /// Gets or sets the AddUserAccountCommand
        /// </summary>
        public RelayCommand AddUserAccountCommand { get; set; }

        /// <summary>
        /// Gets or sets the BatchAddStockCommand
        /// </summary>
//        public RelayCommand BatchAddStockCommand { get; set; }

        /// <summary>
        /// Gets or sets the BatchDeleteCommand
        /// </summary>
//        public RelayCommand BatchDeleteCommand { get; set; }

        /// <summary>
        /// Gets or sets the CloseCommand
        /// </summary>
        public RelayCommand CloseCommand { get; set; }

        /// <summary>
        /// Gets or sets the DeleteUserAccountCommand
        /// </summary>
        public RelayCommand DeleteUserAccountCommand { get; set; }

        /// <summary>
        /// Gets or sets the EditUserAccountCommand
        /// </summary>
        public RelayCommand EditUserAccountCommand { get; set; }

        /// <summary>
        /// Gets the ListOfUserAccounts
        /// </summary>
//        public List<UserAccount> ListOfUserAccounts
//        {
//            get { return UserAccountList.Where(c => c.IsSelected == true).ToList(); }
//        }

        /// <summary>
        /// Gets or sets the UserAccountList
        /// </summary>
        public List<UserAccount> UserAccountList
        {get { return _useraccountList; }
            set { Set(ref _useraccountList, value); }
        }

        /// <summary>
        /// Gets or sets the SearchText
        /// </summary>
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                Set(ref _searchText, value);
                if (String.IsNullOrEmpty(SearchText))
                {
                    UserAccountList = OriginalUserAccountList;
                }
                else
                {
                    UserAccountList = OriginalUserAccountList.Where(c =>
                            c.Name.ToLower().Contains(SearchText.Trim().ToLower()) ||
                            SearchText.Trim().Contains(c.Id.ToString()) ||
                            c.Username.ToLower().Contains(SearchText.Trim().ToLower())                                   
                            )
                        .ToList();
                }
            }
        }

        /// <summary>
        /// Gets or sets the SelectedUserAccount
        /// </summary>
        public UserAccount SelectedUserAccount
        {
            get { return _selectedUserAccount; }
            set { Set(ref _selectedUserAccount, value); }
        }

        /// <summary>
        /// Gets or sets the TotalCount
        /// </summary>
        public int TotalCount
        {
            get { return _totalCount; }
            set { Set(ref _totalCount, value); }
        }

        /// <summary>
        /// Gets or sets the ViewUserAccountCommand
        /// </summary>
        public RelayCommand ViewUserAccountCommand { get; set; }

        /// <summary>
        /// The DoRefresh
        /// </summary>
        /// <param name="obj">The <see cref="RefreshMessage"/></param>
        private void DoRefresh(RefreshMessage obj)
        {
            LoadData();
        }

        /// <summary>
        /// The LoadCommands
        /// </summary>
        private void LoadCommands()
        {
            CloseCommand = new RelayCommand(OnClose);

//            ViewUserAccountCommand = new RelayCommand(OnView);
           AddUserAccountCommand = new RelayCommand(OnAddUserAccount);
//            EditUserAccountCommand = new RelayCommand(OnEdit);
//            DeleteUserAccountCommand = new RelayCommand(OnDelete);
//            BatchAddStockCommand = new RelayCommand(OnBatchAddStock, () => (UserAccountList.Count(c => c.IsSelected) > 0));
//            BatchDeleteCommand = new RelayCommand(OnBatchDelete, () =>
//                (UserAccountList.Count(c => c.IsSelected) > 0)
//            );
        }

        /// <summary>
        /// The LoadData
        /// </summary>
        private void LoadData()
        {
            OriginalUserAccountList = _context.Users.ToList();
            UserAccountList = OriginalUserAccountList;
            TotalCount = UserAccountList.Count;
        }/// <summary>
         /// The OnAddUserAccount
         /// </summary>
        private async void OnAddUserAccount()
        {
            await DialogHost.Show(new AddUserAccountView());
        }

        /// <summary>
        /// The OnBatchAddStock
        /// </summary>
        //        private async void OnBatchAddStock()
        //        {
        //            ServiceLocator.Current.GetInstance<AddStockViewModel>();
        //            MessengerInstance.Send(new ListOfUserAccountsMessage() { UserAccounts = ListOfUserAccounts });
        //            await DialogHost.Show(new AddStockView(), "RootDialog");
        //        }

        /// <summary>
        /// The OnBatchDelete
        /// </summary>
        //        private async void OnBatchDelete()
        //        {
        //            await DialogHost.Show(new MessageView(), "RootDialog", delegate (object sender, DialogClosingEventArgs args)
        //            {
        //                if (Equals(args.Parameter, false)) return;
        //
        //                if (Equals(args.Parameter, true))
        //                {
        //                    var list = UserAccountList.Where(c => c.IsSelected).ToList();
        //                    _context.Users.RemoveRange(list);
        //                    _context.SaveChanges();
        //                    DoRefresh(null);
        //                }
        //            });
        //        }

        /// <summary>
        /// The OnClose
        /// </summary>
        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
        }

        /// <summary>
        /// The OnDelete
        /// </summary>
        private async void OnDelete()
        {
            await DialogHost.Show(new MessageView(), "RootDialog", delegate (object sender, DialogClosingEventArgs args)
            {
                if (Equals(args.Parameter, false)) return;

                if (Equals(args.Parameter, true))
                {
                    _context.Users.Remove(SelectedUserAccount);
                    _context.SaveChanges();
                    DoRefresh(null);
                }
            });
        }

      
//        private async void OnEdit()
//        {
//            ServiceLocator.Current.GetInstance<EditUserAccountViewModel>();
//            MessengerInstance.Send(new SelectedUserAccountMessage() { SelectedUserAccount = SelectedUserAccount });
//            await DialogHost.Show(new EditUserAccountView());
//        }

        /// <summary>
        /// The OnView
        /// </summary>
//        private async void OnView()
//        {
//            await DialogHost.Show(new UserAccountView() { DataContext = this });
//        }
    }
}
