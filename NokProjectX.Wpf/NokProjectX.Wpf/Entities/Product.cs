namespace NokProjectX.Wpf.Entities
{
    /// <summary>
    /// Defines the <see cref="Product" />
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the Image
        /// </summary>
        public byte[] Image { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether IsSelected
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Price
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Gets or sets the ProductCode
        /// </summary>
        public int ProductCode { get; set; }

        /// <summary>
        /// Gets or sets the Stock
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Gets or sets the Type
        /// </summary>
        public virtual Type Type { get; set; }
    }
}
