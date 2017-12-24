using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DevExpress.Printing;
using DevExpress.Xpf.Printing;
using DevExpress.XtraPrinting;
using DevExpress.XtraReports;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using NokProjectX.Wpf.Context;
using NokProjectX.Wpf.Entities;
using NokProjectX.Wpf.Reports;


namespace NokProjectX.Wpf.ViewModel.Reports
{
    public class ReportViewModel : ViewModelBase 
    {
        private readonly YumiContext _context;
        private object _reportMode;
        private List<Entities.Transaction> _transactions;
        private List<Entities.Transaction> _originalTransactions;
        private DateTime _startDate;
        private DateTime _endDate;

        public ReportViewModel(YumiContext context)
        {
            _context = context;
            StartDate = DateTime.Now ;
            EndDate = DateTime.Now ;
            EndDate = EndDate.AddDays(1);
            LoadData();
            PrintCommand = new RelayCommand(OnPrint);
        }

        private void OnPrint()
        {
            var report = new TransactionReport();
            report.DataSource = _context.Invoices.ToList();
            
            var window = new DocumentPreviewWindow();
            window.PreviewControl.DocumentSource = report;
            report.CreateDocument(true);
            report.PrintingSystem.Document.AutoFitToPagesWidth = 1;
            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.WindowState = WindowState.Maximized;
            window.Style = null;
            window.ShowDialog();
        }

        public RelayCommand PrintCommand { get; set; }

        private void LoadData()
        {
            Customers = _context.Customers.ToList();
        }

        public object ReportMode
        {
            get { return _reportMode; }
            set
            {
                Set(ref _reportMode, value);
                if (value != null)
                {
                    var a = value as ListBoxItem;
                    if (a != null && a.Name == "All")
                    {
                        IsByCustomer = false;
                        IsByDate = false;
                        Transactions = _context.Transactions.ToList();
                        CalculateTransaction();
                    }
                    if (a != null && a.Name == "Date")
                    {
                        IsByCustomer = false;
                        IsByDate = true;
                        Transactions = _context.Transactions.Where(c => c.Date > StartDate && c.Date < EndDate).ToList();
                        CalculateTransaction();
                    }
                    if (a != null && a.Name == "Customer")
                    {
                        if (SelectedCustomer != null)
                        {
                            Invoices = _context.Invoices.Where(c => c.Customer.Id == SelectedCustomer.Id).ToList();
                        }
                        IsByCustomer = true;
                        IsByDate = false;
                    }
                }
            }
        }

        void CalculateTransaction()
        {
            TotalPayment = 0;
            Credit = 0;
            TotalSales = 0;
            foreach (var transaction in Transactions)
            {
                TotalPayment = TotalPayment + transaction.Payment;
                Credit = Credit + (transaction.Balance);
                TotalSales = TotalSales + transaction.TotalPrice;
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
                if (value != null)
                {
                    Invoices = _context.Invoices.Where(c => c.Customer.Id == SelectedCustomer.Id).ToList();
                }
                else
                {
                    Invoices = null;
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
            set { Set(ref _transactions, value); }
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
        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                Set(ref _startDate, value);
                Transactions = _context.Transactions.Where(c => c.Date > StartDate && c.Date <= EndDate).ToList();
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                Set(ref _endDate, value);
                Transactions = _context.Transactions.Where(c => c.Date > StartDate && c.Date <= EndDate).ToList();
            }
        }
    }
}