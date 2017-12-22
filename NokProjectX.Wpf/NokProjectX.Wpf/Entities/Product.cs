using System.ComponentModel.DataAnnotations.Schema;

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
        
        public string CodeString { get; set; }
        public int CodeNumber { get; set; }

        public int Stock { get; set; }

        
        public virtual Type Type { get; set; }
        
        public string ProductCode { get { return CodeString + CodeNumber; } }
    }
}
