using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NokProjectX.Wpf.Context;
using System.Windows;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using MvvmValidation;
using MySql.Data.MySqlClient;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Entities;
using NokProjectX.Wpf.Views.Common;
using ViewModelBase = GalaSoft.MvvmLight.ViewModelBase;

namespace NokProjectX.Wpf.ViewModel.About
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly YumiContext _context;
        public RelayCommand ExportCommand { get; set; }
        public RelayCommand ImportCommand { get; set; }
        public AboutViewModel(YumiContext context)
        {
            _context = context;
            ExportCommand = new RelayCommand(OnExport);
           ImportCommand = new RelayCommand(OnImport);

        }

        private async void OnExport()
        {
            Backup();
            await DialogHost.Show(new SuccessView(), "RootDialog");
        }
        private async void OnImport()
        {
            Restore();
            await DialogHost.Show(new SuccessView(), "RootDialog");}
        
        private void Backup()
        {
            string constring = "Server=localhost;Port=3307;Database=projectx;Uid=real;Pwd=real";
            string file = "C:\\xampp\\htdocs\\backup.sql";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn; conn.Open();
                        mb.ExportToFile(file);
                        conn.Close();
                    }
                }
            }
        }

        private void Restore()
        {
            string constring = "Server=localhost;Port=3307;Database=projectx;Uid=real;Pwd=real";
            string file = "C:\\xampp\\htdocs\\backup.sql";
            using (MySqlConnection conn = new MySqlConnection(constring))
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    using (MySqlBackup mb = new MySqlBackup(cmd))
                    {
                        cmd.Connection = conn;
                        conn.Open();
                        mb.ImportFromFile(file);
                        conn.Close();
                    }
                }
            }
        }
    }
}
