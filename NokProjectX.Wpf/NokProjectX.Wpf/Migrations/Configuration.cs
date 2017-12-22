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



            var product1 = new Product()
            {
                Name = "Tarpaulin",
                Description = "Standard",
                Price = 10.0d,
                CodeString = "TARP",
                CodeNumber = 1000001,
                Stock = 100,
                Type = ft,
            };
            var product2 = new Product()
            {
                Name = "Tarpaulin",
                Description = "Regular",
                Price = 15.0d,
                CodeString = "TARP",
                CodeNumber = 1000002,
                Stock = 100,
                Type = ft,
            };
            var product3 = new Product()
            {
                Name = "Tracing Paper 20 x 30",
                Description = "Lines",
                Price = 90.0d,
                CodeString = "TRAC",
                CodeNumber = 1000003,
                Stock = 100,
                Type = pcs,
            };
            var product4 = new Product()
            {
                Name = "Tracing Paper 24 x 34",
                Description = "Graphics",
                Price = 120.0d,
                CodeString = "TRAC",
                CodeNumber = 1000004,
                Stock = 100,
                Type = pcs,
            };

            context.Products.AddOrUpdate(c => c.CodeNumber,
                product1,
                product2,
                product3,
                product4
                
            );
            
        }
    }
}
