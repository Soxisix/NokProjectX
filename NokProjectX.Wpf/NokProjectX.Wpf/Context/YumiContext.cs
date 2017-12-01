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
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<YumiContext>());
        }

        public DbSet<Product> Products { get; set; }
    }
}