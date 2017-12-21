using System;

namespace NokProjectX.Wpf.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public double Payment { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual Customer Customer { get; set; }
    }
}