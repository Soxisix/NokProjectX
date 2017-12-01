namespace NokProjectX.Wpf.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int ProductCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string Type { get; set; }
        public double Price { get; set; }

    }
}