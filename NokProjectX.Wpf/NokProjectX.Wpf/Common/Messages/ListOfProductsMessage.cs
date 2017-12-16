namespace NokProjectX.Wpf.Common.Messages
{
    using NokProjectX.Wpf.Entities;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="ListOfProductsMessage" />
    /// </summary>
    public class ListOfProductsMessage
    {
        /// <summary>
        /// Gets or sets the Products
        /// </summary>
        public List<Product> Products { get; set; }
    }
}
