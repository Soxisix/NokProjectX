namespace NokProjectX.Wpf.Entities
{
    /// <summary>
    /// Defines the <see cref="Product" />
    /// </summary>
    public class Product
    {
        
        public string Description { get; set; }
        
        public int Id { get; set; }
        
        public byte[] Image { get; set; }
        
        public bool IsSelected { get; set; }
        
        public string Name { get; set; }
        
        public double Price { get; set; }
        
        public int ProductCode { get; set; }

        public int Stock { get; set; }

        
        public virtual Type Type { get; set; }
    }
}
