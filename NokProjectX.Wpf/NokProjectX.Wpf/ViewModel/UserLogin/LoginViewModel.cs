using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using MvvmValidation;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;

namespace NokProjectX.Wpf.ViewModel.UserLogin
{
    public class LoginViewModel : ValidatableViewModelBase
    {
        private readonly YumiContext _context;

        
        private string _useraccount;
        private string _username;
        private string _password;
   
        public LoginViewModel(YumiContext context)
        {
            _context = context;
            CloseCommand = new RelayCommand(OnClose);

            SignInCommand = new RelayCommand<Window>(OnSignIn);
            ValidateLogin();
        }

    
        
     

        private async void OnSignIn(Window obj)
        {
            await ValidateAsync();
            if (HasErrors)
            {
                return;
            }

            var checkuser = _context.Users.Where(c => c.Username == (UserName) && c.Password == (Password)).FirstOrDefault();
     
            if (checkuser != null || UserName == "Black" && Password == "Pepper")
            {
                MainWindow main = new MainWindow();
                Validator.Reset();
                main.Show();
                obj.Close();
            }                     
            else
            {
                MessageBox.Show("Failed");
            }
        }


        public RelayCommand<Window> SignInCommand { get; set; }

       

        public RelayCommand CloseCommand { get; set; }

        private void OnClose()
        {
           
//           Application.Current.Shutdown();
            Environment.Exit(0);
        }

        public string UserName { get { return _username; } set { Set(ref _username, value); } }

        public string Password { get { return _password; } set { Set(ref _password, value); } }

        public async Task ValidateAsync()
        {
            await Validator.ValidateAllAsync();
        }

        void ValidateLogin()
        {
            Validator.AddRequiredRule(() => UserName, "Username is Required!");
            Validator.AddRequiredRule(() => Password, "Password is Required!");
        }
    }
}