using NokProjectX.Wpf.Entities;

namespace NokProjectX.Wpf.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<NokProjectX.Wpf.Context.YumiContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(NokProjectX.Wpf.Context.YumiContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            var pcs = new Entities.Type { Name = "pcs" };

            var ft = new Entities.Type { Name = "ft" };

            context.Types.AddOrUpdate(c => c.Name,

                pcs,

                ft

            );



            var product1 = new Product() { };

            context.Products.AddOrUpdate(c => c.ProductCode

            );

            context.SaveChanges();
        }
    }
}
