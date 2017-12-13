using System.Data.Entity;
using MySql.Data.Entity;
using NokProjectX.Wpf.Entities;

namespace NokProjectX.Wpf.Context
{
    [DbConfigurationType(typeof(MySqlEFConfiguration))]
    public class YumiContext : DbContext
    {
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

        public DbSet<Product> Products { get; set; }
        public DbSet<Type> Types { get; set; }
    }
}