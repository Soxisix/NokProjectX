using System;

namespace NokProjectX.Wpf.Entities
{
    /// <summary>
    /// Defines the <see cref="Invoice" />
    /// </summary>
    public class Invoice
    {
        public int Id { get; set; }

        public int InvoiceCode { get; set; }
        public double Price { get; set; }

        public double Unit { get; set; }

        public DateTime Date { get; set; }

        public virtual Product Product { get; set; }
        
        public int Quantity { get; set; }

        public string Size { get; set; }

        public string Description { get; set; }

        public double TotalPrice { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
