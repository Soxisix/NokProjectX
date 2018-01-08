using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NokProjectX.Wpf.Context;
using System.Windows.Forms;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using MvvmValidation;
using MySql.Data.MySqlClient;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Entities;
using NokProjectX.Wpf.Views.Common;
using RSMT.Views.Common;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using ViewModelBase = GalaSoft.MvvmLight.ViewModelBase;

namespace NokProjectX.Wpf.ViewModel.About
{
    public class AboutViewModel : ViewModelBase
    {
        private readonly YumiContext _context;
        public RelayCommand ExportCommand { get; set; }
        public RelayCommand ImportCommand { get; set; }

        string constring = "Server=192.168.0.101;Port=3307;Database=projectx;Uid=real;Pwd=real;charset=utf8";
        public AboutViewModel(YumiContext context)
        {
            _context = context;
            ExportCommand = new RelayCommand(OnExport);
           ImportCommand = new RelayCommand(OnImport);

        }

        private async void OnExport()
        {
            Backup();
            
        }
        private async void OnImport()
        {
            Restore();
            
        }
        
        private async void Backup()
        {
//            string constring = "Server=localhost;Port=3307;Database=projectx;Uid=root;Pwd=admin;charset=utf8";
//            string file = "C:\\backup.sql";

            SaveFileDialog fileDialog = new SaveFileDialog();
            fileDialog.Filter = "Sql files (*.Sql)|*.Sql";
            fileDialog.FileName = "backup" + DateTime.Now.ToString("MM-dd-yyyy") + ".sql";
            var result = fileDialog.ShowDialog();
            if (result == false)
            {
                return;
            }
            await DialogHost.Show(new PleaseWaitView(), "RootDialog",
                delegate (object sender, DialogOpenedEventArgs args)
                {
                    
                    Task.Run(() =>
                    {
                        try
                        {
                            using (MySqlConnection conn = new MySqlConnection(constring))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    using (MySqlBackup mb = new MySqlBackup(cmd))
                                    {

                                        cmd.Connection = conn; conn.Open();

                                        mb.ExportToFile(fileDialog.FileName);
                                        conn.Close();
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }).ContinueWith((t, _) =>
                    {
                       args.Session.UpdateContent(new SuccessView());
                    }, null, TaskScheduler.FromCurrentSynchronizationContext());
                });

            
            
        }

        private async void Restore()
        {
            
            //            string file = "C:\\backup.sql";
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Sql files (*.Sql)|*.Sql";
            var result = fileDialog.ShowDialog();
            if (result == false)
            {
                return;
            }
            await DialogHost.Show(new PleaseWaitView(), "RootDialog",
                delegate (object sender, DialogOpenedEventArgs args)
                {

                    Task.Run(() =>
                    {
                        try
                        {
                            using (MySqlConnection conn = new MySqlConnection(constring))
                            {
                                using (MySqlCommand cmd = new MySqlCommand())
                                {
                                    using (MySqlBackup mb = new MySqlBackup(cmd))
                                    {

                                        cmd.Connection = conn; conn.Open();

                                        mb.ImportFromFile(fileDialog.FileName);
                                        conn.Close();
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    }).ContinueWith((t, _) =>
                    {
                        args.Session.UpdateContent(new SuccessView());
                    }, null, TaskScheduler.FromCurrentSynchronizationContext());
                });

        }
    }
}
