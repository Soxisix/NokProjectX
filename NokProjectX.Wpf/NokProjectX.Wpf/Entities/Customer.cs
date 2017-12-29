using System.Collections.Generic;

namespace NokProjectX.Wpf.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public bool IsSelected { get; set; }
        public virtual List<Transaction> Transactions { get; set; }
    }
}