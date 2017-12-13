namespace NokProjectX.Wpf.Entities
{
    public class Invoice
    {
        public int Id { get; set; }

        public virtual Product Product { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Size { get; set; }
    }
}