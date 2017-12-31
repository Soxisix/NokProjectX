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
        
    
     
        private readonly YumiContext _context;

        private List<UserAccount> _useraccountList;

        private List<Customer> _customeraccountList;

        private string _searchText;

        private UserAccount _selectedUserAccount;

        private Customer _selectedCustomerAccount;

        private int _totalCount;

        private List<UserAccount> OriginalUserAccountList;

        private List<Customer> OriginalCustomerList;

        private List<string> _modeList;

        private bool _user;

        private string _selectedMode;

        public RelayCommand AddUserAccountCommand { get; set; }

        public RelayCommand AddCustomerCommand { get; set; }

        public RelayCommand BatchDeleteCommand { get; set; }
    

        public RelayCommand CloseCommand { get; set; }

        public RelayCommand DeleteUserAccountCommand { get; set; }

        public RelayCommand EditUserAccountCommand { get; set; }
        public RelayCommand EditCustomerCommand { get; set; }
        public RelayCommand DeleteCustomerCommand { get; set; }

        /// <param name="context">The <see cref="YumiContext"/></param>
        public SettingsViewModel(YumiContext context)
        {
            _context = context;
            MessengerInstance.Register<RefreshMessage>(this, DoRefresh);
            LoadData();
            ModeList = new List<string>()
            {
                "User Accounts",
                "Customer Accounts"
            };
            LoadCommands();
        }

     
        public bool IsByUser
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }

     

        public string SelectedMode
        {
            get { return _selectedMode; }
            set
            {
                Set(ref _selectedMode, value);
                if (!String.IsNullOrEmpty(value))
                {
                    if (value == "User Accounts")
                    {
                        IsByUser = true;

                      
                            TotalCount = UserAccountList.Count;
   
                    }
                    else
                    {
                        IsByUser =false;
                        TotalCount = CustomerList.Count;}
                }
            }
        }

 

        public List<string> ModeList
        {
            get { return _modeList; }
            set { Set(ref _modeList, value); }
        }  

       
       

        public List<UserAccount> UserAccountList
        {get { return _useraccountList; }
            set { Set(ref _useraccountList, value); }
        }

        public List<Customer> CustomerList
        {
            get { return _customeraccountList; }
            set { Set(ref _customeraccountList, value); }
        }

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                Set(ref _searchText, value);
                if (String.IsNullOrEmpty(SearchText))
                {
                    if (IsByUser == true)
                    {
                        UserAccountList = OriginalUserAccountList;
                    }
                    else
                    {
                        CustomerList = OriginalCustomerList;
                    }
               
                }
                else
                {
                    if (IsByUser == true)
                    {
                        UserAccountList = OriginalUserAccountList.Where(c =>
                                c.Name.ToLower().Contains(SearchText.Trim().ToLower()) ||
                                SearchText.Trim().Contains(c.Id.ToString()) ||
                                c.Username.ToLower().Contains(SearchText.Trim().ToLower())
                            )
                            .ToList();
                    }
                    else
                    {
                        CustomerList = CustomerList = OriginalCustomerList.Where(c =>
                                c.Name.ToLower().Contains(SearchText.Trim().ToLower()) ||
                                SearchText.Trim().Contains(c.Id.ToString())
                              
                            )
                            .ToList();

                    }
                }
            }
        }

     
        public UserAccount SelectedUserAccount
        {
            get { return _selectedUserAccount; }
            set
            {
                Set(ref _selectedUserAccount, value);
                BatchDeleteCommand.RaiseCanExecuteChanged();}
        }

        public Customer SelectedCustomer
        {
            get { return _selectedCustomerAccount; }
            set
            {
                Set(ref _selectedCustomerAccount, value);
                BatchDeleteCommand.RaiseCanExecuteChanged();
            }
        }

        public int TotalCount
        {
            get { return _totalCount; }
            set { Set(ref _totalCount, value); }
        }


       

   
        /// <param name="obj">The <see cref="RefreshMessage"/></param>
        private void DoRefresh(RefreshMessage obj)
        {
            LoadData();
        }
        private void LoadCommands()
        {
            CloseCommand = new RelayCommand(OnClose);
            if (IsByUser == true)
            {
                EditUserAccountCommand = new RelayCommand(OnEditUserAccount);

                BatchDeleteCommand = new RelayCommand(OnUserAccountBatchDelete, () =>
                    (UserAccountList.Count(c => c.IsSelected) > 0));
                DeleteUserAccountCommand = new RelayCommand(OnDelete);


            }
            else
            {
                EditCustomerCommand = new RelayCommand(OnEditCustomer);
                BatchDeleteCommand = new RelayCommand(OnCustomerBatchDelete, () =>
                    (CustomerList.Count(c => c.IsSelected) > 0));
                DeleteCustomerCommand = new RelayCommand(OnDelete);

            }


           

        

        
            AddUserAccountCommand = new RelayCommand(OnAddUserAccount);
         
                
        }

   
        private void LoadData()
        {

            
                OriginalUserAccountList = _context.Users.ToList();
                UserAccountList = OriginalUserAccountList;
         
            OriginalCustomerList = _context.Customers.ToList();
                CustomerList = OriginalCustomerList;
            

         
             }

        private async void OnAddUserAccount()
        {
            if (IsByUser == true)
            {
                await DialogHost.Show(new AddUserAccountView()
                
                );
              
                TotalCount = UserAccountList.Count();
            }
            else
            {
                await DialogHost.Show(new AddCustomerView());

                    TotalCount = CustomerList.Count;
                
            }
            LoadData();
        }


        private async void OnUserAccountBatchDelete()
        {
            await DialogHost.Show(new MessageView(), "RootDialog", delegate (object sender, DialogClosingEventArgs args)
            {
                if (Equals(args.Parameter, false)) return;

                if (Equals(args.Parameter, true))
                {
                    var list = UserAccountList.Where(c => c.IsSelected).ToList();
                    _context.Users.RemoveRange(list);
                    _context.SaveChanges();
                    DoRefresh(null);
                 
                }
            });
            LoadData();
            if (IsByUser == true)
            {
                TotalCount = UserAccountList.Count();
            }
            else
            {
                TotalCount = CustomerList.Count();
            }
        }

        private async void OnCustomerBatchDelete()
        {
            await DialogHost.Show(new MessageView(), "RootDialog", delegate (object sender, DialogClosingEventArgs args)
            {
                if (Equals(args.Parameter, false)) return;

                if (Equals(args.Parameter, true))
                {
                    var list = CustomerList.Where(c => c.IsSelected).ToList();
                    _context.Customers.RemoveRange(list);
                    _context.SaveChanges();
                    DoRefresh(null);
                    }
            });
            LoadData();
            if (IsByUser == true)
            {
                TotalCount = UserAccountList.Count();
            }
            else
            {
                TotalCount = CustomerList.Count();
            }

        }




        private void OnClose()
        {DialogHost.CloseDialogCommand.Execute(this, null);
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
                    if (IsByUser == true)
                    {
                        _context.Users.Remove(SelectedUserAccount);
                    }
                    else
                    {
                        _context.Customers.Remove(SelectedCustomer);
                    }
                   
                    _context.SaveChanges();
                    DoRefresh(null);
                }
            });

            if (IsByUser == true)
            {
                TotalCount = UserAccountList.Count();
            }
            else
            {
                TotalCount = CustomerList.Count();
            }
        }




        private async void OnEditUserAccount()
        {
         
            ServiceLocator.Current.GetInstance<EditUserAccountViewModel>();
            MessengerInstance.Send(new SelectedUserMessage() { SelectedUser = SelectedUserAccount });
            await DialogHost.Show(new EditUserAccountView());
        }

        private async void OnEditCustomer()
        {

            ServiceLocator.Current.GetInstance<EditCustomerViewModel>();
            MessengerInstance.Send(new SelectedCustomerMessage() { SelectedCustomer = SelectedCustomer });
            await DialogHost.Show(new EditCustomerView());
        }



    }
}
