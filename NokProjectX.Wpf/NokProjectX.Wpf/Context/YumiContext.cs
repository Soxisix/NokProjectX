﻿namespace NokProjectX.Wpf.Context
{
    using MySql.Data.Entity;
    using NokProjectX.Wpf.Entities;
    using System.Data.Entity;

    /// <summary>
    /// Defines the <see cref="YumiContext" />
    /// </summary>
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class YumiContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YumiContext"/> class.
        /// </summary>
        public YumiContext() : base("name=YumiDb")
        {
            if (Database.Exists())
            {
                Database.SetInitializer(new DropCreateDatabaseIfModelChanges<YumiContext>());
            }
            else
            {
                Database.SetInitializer(new CreateDatabaseIfNotExists<YumiContext>());
            }
        }

        /// <summary>
        /// Gets or sets the Products
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Gets or sets the Types
        /// </summary>
        public DbSet<Type> Types { get; set; }
    }
}
