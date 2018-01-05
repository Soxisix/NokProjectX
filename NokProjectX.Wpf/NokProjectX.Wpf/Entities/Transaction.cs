using System;
using System.Collections.Generic;

namespace NokProjectX.Wpf.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public double Payment { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
        public virtual Customer Customer { get; set; }

        public int TransactionNumber { get; set; }
        public double Balance { get { return TotalPrice - Payment; } }

        public virtual UserAccount UserAccount { get; set; }
    }
}