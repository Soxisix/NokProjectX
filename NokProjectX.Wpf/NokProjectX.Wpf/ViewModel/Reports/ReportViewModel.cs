using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Printing;
using DevExpress.Xpf.Docking;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports;
using DevExpress.XtraReports.UI;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MaterialDesignThemes.Wpf;
using MvvmValidation;
using NokProjectX.Wpf.Common.Messages;
using NokProjectX.Wpf.Common.Validator;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;
using NokProjectX.Wpf.Reports;
using NokProjectX.Wpf.Views.Reports;
using static System.Data.Entity.DbFunctions;

namespace NokProjectX.Wpf.ViewModel.Reports
{
    public class ReportViewModel : ValidatableViewModelBase
    {
        private readonly YumiContext _context;
        private string _reportMode;
        private List<Entities.Transaction> _transactions;
        private List<Entities.Transaction> _originalTransactions;
        private DateTime? _startDate;
        private DateTime? _endDate;

        public ReportViewModel(YumiContext context)
        {
            _context = context;
//            StartDate = DateTime.Now ;
//            EndDate = DateTime.Now ;
//            EndDate = EndDate.AddDays(1);
            ConfigureRules();

            CloseDialogCommand = new RelayCommand(OnClose);
            ViewTransactionCommand = new RelayCommand(OnViewTransaction);
            OkCommand = new RelayCommand(OnOk);
            UpdateCommand = new RelayCommand(OnUpdate, () => SelectedTransaction != null && SelectedTransaction.Balance > 0);
            PrintCommand = new RelayCommand(OnPrint, () => Transactions != null);
            var modeList = new List<string> { "All Transactions", "By Customer" };
            ModeList = modeList;
            PaymentModeList = new List<string> { "All","Paid", "Unpaid" };
            MessengerInstance.Register<RefreshMessage>(this, OnRefresh);
            LoadData();
        }

        private string _selectedPaymentMode;

        public string SelectedPaymentMode
        {
            get { return _selectedPaymentMode; }
            set
            {
                Set(ref _selectedPaymentMode, value);
                FilterAllTransactions();
            }
        }
        private List<string> _paymentModeList;

        public List<string> PaymentModeList
        {
            get { return _paymentModeList; }
            set { Set(ref _paymentModeList, value); }
        }
        public RelayCommand CloseDialogCommand { get; set; }

        private void OnClose()
        {
            DialogHost.CloseDialogCommand.Execute(this, null);
        }

        public RelayCommand ViewTransactionCommand { get; set; }

        private async void OnViewTransaction()
        {
            Invoices = SelectedTransaction.Invoice.ToList();
            await DialogHost.Show(new SelectedTransactionView() {DataContext = this});
        }

        
        public async Task ValidateAsync()
        {
            await Validator.ValidateAllAsync();
        }
        void ConfigureRules()
        {
            Validator.AddAsyncRule(() => UpdatePayment, async () =>
            {
                if (SelectedTransaction.Balance < UpdatePayment || UpdatePayment == null)
                {
                    return RuleResult.Invalid("Invalid Payment");
                }
                else
                {
                    return RuleResult.Valid();
                }
            });
        }

        public RelayCommand OkCommand { get; set; }

        private async void OnOk()
        {
           await ValidateAsync();
            if (HasErrors)
            {
                return;
            }

            Entities.Transaction selectedTransaction = SelectedTransaction;
            SelectedTransaction.Payment += UpdatePayment.GetValueOrDefault();
            _context.SaveChanges();
            UpdatePayment = null;
            LoadData();
            if (_isByCustomer)
            {
                Transactions = _originalTransactions.Where(c => c.Customer.Id == SelectedCustomer.Id).ToList();
                SelectedTransaction = selectedTransaction;
            }
            else
            {
                Transactions = _originalTransactions;
                SelectedTransaction = selectedTransaction;

            }
            UpdateCommand.RaiseCanExecuteChanged();
            CalculateTransaction();
            DialogHost.CloseDialogCommand.Execute(this, null);
            
        }

        private double? _updatePayment;
        public double? UpdatePayment
        {
            get { return _updatePayment; }
            set { Set(ref _updatePayment, value); }
        }

        public RelayCommand UpdateCommand { get; set; }

        private async void OnUpdate()
        {
            await DialogHost.Show(new UpdateTransactionView() {DataContext = this}, "RootDialog",
                delegate(object sender, DialogClosingEventArgs args)
                {
                    if (Equals(args.Parameter, false))
                    {
                        UpdatePayment = null;
                    }
                });
        }

        private Entities.Transaction _selectedTransaction;

        public Entities.Transaction SelectedTransaction
        {
            get { return _selectedTransaction; }
            set
            {
                Set(ref _selectedTransaction, value);
                UpdateCommand.RaiseCanExecuteChanged();
            }
        }
        
        private void OnRefresh(RefreshMessage obj)
        {
            LoadData();
        }

        private void OnPrint()
        {
            XtraReport report;
            if (IsByCustomer)
            {
                report = new SalesReport();
                if (SelectedCustomer != null)
                {
                    report.DataSource = Transactions.Where(c => c.Customer.Id == SelectedCustomer.Id).ToList();
                }
            }
            else
            {
                report = new AllTransactionReport();
                report.DataSource = Transactions;
            }
            
            var window = new DocumentPreviewWindow();
            window.PreviewControl.DocumentSource = report;
            report.CreateDocument(true);
            report.PrintingSystem.Document.AutoFitToPagesWidth = 1;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowState = WindowState.Maximized;
            window.ShowDialog();
        }

        public RelayCommand PrintCommand { get; set; }

        private void LoadData()
        {
            _originalTransactions = _context.Transactions.ToList();

            Transactions = _originalTransactions;
            Customers = _context.Customers.ToList();
            
        }

        public string ReportMode
        {
            get { return _reportMode; }
            set
            {
                Set(ref _reportMode, value);
                if (value != null)
                {
                    if ((string) value == "All Transactions")
                    {
                        IsByCustomer = false;
                        FilterAllTransactions();
                        
                    }
                    if ((string) value == "By Customer")
                    {
                        IsByCustomer = true;
                        if (SelectedCustomer != null)
                        {
                            FilterAllTransactions();
                        }
                        
                    }
                    StartDate = null;
                    EndDate = null;
                }
            }
        }

        private void FilterAllTransactions()
        {
            if (ReportMode == "All Transactions")
            {
                if (StartDate != null && EndDate != null && SelectedPaymentMode == "All")
                {
                    Transactions = _originalTransactions.Where(c => c.Date.Date >= StartDate && c.Date.Date <= EndDate).ToList();
                }
                if (StartDate != null && EndDate != null && SelectedPaymentMode == "Paid")
                {
                    Transactions = _originalTransactions.Where(c => c.Date.Date >= StartDate && c.Date.Date <= EndDate && Math.Abs(c.TotalPrice - c.Payment) < 1).ToList();
                }
                if (StartDate != null && EndDate != null && SelectedPaymentMode == "Unpaid")
                {
                    Transactions = _originalTransactions.Where(c => c.Date.Date >= StartDate && c.Date.Date <= EndDate && Math.Abs(c.TotalPrice - c.Payment) < 0).ToList();
                }
                if ((StartDate == null || EndDate == null) && SelectedPaymentMode == "All")
                {
                    Transactions = _originalTransactions.ToList();
                }
                if ((StartDate == null || EndDate == null) && SelectedPaymentMode == "Paid")
                {
                    Transactions = _originalTransactions.Where(c => Math.Abs(c.TotalPrice - c.Payment) < 1).ToList();
                }
                if ((StartDate == null || EndDate == null) && SelectedPaymentMode == "Unpaid")
                {
                    Transactions = _originalTransactions.Where(c => Math.Abs(c.TotalPrice - c.Payment) < 0).ToList();
                }
            }
            else
            {
                if (SelectedCustomer != null)
                {
                    if (StartDate != null && EndDate != null && SelectedPaymentMode == "All")
                    {
                        Transactions = _originalTransactions.Where(c => c.Customer.Id == SelectedCustomer.Id && c.Date.Date >= StartDate && c.Date.Date <= EndDate).ToList();
                    }
                    if (StartDate != null && EndDate != null && SelectedPaymentMode == "Paid")
                    {
                        Transactions = _originalTransactions.Where(c => c.Customer.Id == SelectedCustomer.Id && c.Date.Date >= StartDate && c.Date.Date <= EndDate && Math.Abs(c.TotalPrice - c.Payment) < 1).ToList();
                    }
                    if (StartDate != null && EndDate != null && SelectedPaymentMode == "Unpaid")
                    {
                        Transactions = _originalTransactions.Where(c => c.Customer.Id == SelectedCustomer.Id && c.Date.Date >= StartDate && c.Date.Date <= EndDate && Math.Abs(c.TotalPrice - c.Payment) < 0).ToList();
                    }
                    if ((StartDate == null || EndDate == null) && SelectedPaymentMode == "All")
                    {
                        Transactions = _originalTransactions.Where(c => c.Customer.Id == SelectedCustomer.Id).ToList();
                    }
                    if ((StartDate == null || EndDate == null) && SelectedPaymentMode == "Paid")
                    {
                        Transactions = _originalTransactions.Where(c => c.Customer.Id == SelectedCustomer.Id && Math.Abs(c.TotalPrice - c.Payment) < 1).ToList();
                    }
                    if ((StartDate == null || EndDate == null) && SelectedPaymentMode == "Unpaid")
                    {
                        Transactions = _originalTransactions.Where(c => c.Customer.Id == SelectedCustomer.Id && Math.Abs(c.TotalPrice - c.Payment) < 0).ToList();
                    }
                }
            }
            CalculateTransaction();
            
        }

        private List<string> _modeList;

        public List<string> ModeList
        {
            get { return _modeList; }
            set { Set(ref _modeList, value); }
        }

        void CalculateTransaction()
        {
            TotalPayment = 0;
            Credit = 0;
            TotalSales = 0;
            if (Transactions != null)
            {
                foreach (var transaction in Transactions)
                {
                    TotalPayment = TotalPayment + transaction.Payment;
                    Credit = Credit + (transaction.Balance);
                    TotalSales = TotalSales + transaction.TotalPrice;
                }
            }
        }

        void CalculateInvoice()
        {
            TotalSales = 0;
            foreach (var transaction in Invoices)
            {
                Credit = Credit + (transaction.TotalPrice - TotalPayment);
                TotalSales = TotalSales + transaction.TotalPrice;
            }
        }

        private double _totalPayment;

        public double TotalPayment
        {
            get { return _totalPayment; }
            set { Set(ref _totalPayment, value); }
        }

        private double _credit;

        public double Credit
        {
            get { return _credit; }
            set { Set(ref _credit, value); }
        }

        private double _totalSale;

        public double TotalSales
        {
            get { return _totalSale; }
            set { Set(ref _totalSale, value); }
        }

        private List<Invoice> _invoices;

        public List<Invoice> Invoices
        {
            get { return _invoices; }
            set { Set(ref _invoices, value); }
        }

        private Customer _selectedCustomer;

        public Customer SelectedCustomer
        {
            get { return _selectedCustomer; }
            set
            {
                Set(ref _selectedCustomer, value);
                if (value != null && IsByCustomer)
                {
                    FilterAllTransactions();
                }
                if (value == null && !IsByCustomer)
                {
                    Transactions = null;
                    CalculateTransaction();
                }
            }
        }

        private List<Customer> _customers;

        public List<Customer> Customers
        {
            get { return _customers; }
            set { Set(ref _customers, value); }
        }

        public List<Entities.Transaction> Transactions
        {
            get { return _transactions; }
            set
            {
                Set(ref _transactions, value);
                PrintCommand.RaiseCanExecuteChanged();
            }
        }

        private bool _isByCustomer;

        public bool IsByCustomer
        {
            get { return _isByCustomer; }
            set { Set(ref _isByCustomer, value); }
        }

        private bool _isByDate;

        public bool IsByDate
        {
            get { return _isByDate; }
            set { Set(ref _isByDate, value); }
        }

        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                Set(ref _startDate, value);
                FilterAllTransactions();
            }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                Set(ref _endDate, value);
                FilterAllTransactions();
            }
        }
    }
}