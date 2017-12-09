namespace NokProjectX.Wpf.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public virtual Type Type { get; set; }
        public double Price { get; set; }

        public byte[] Image { get; set; }


        public bool IsSelected { get; set; }
    }
}