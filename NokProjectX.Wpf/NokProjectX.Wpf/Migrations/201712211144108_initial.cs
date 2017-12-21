namespace NokProjectX.Wpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                        Mobile = c.String(unicode: false),
                        Address = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Transactions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Payment = c.Double(nullable: false),
                        TotalPrice = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Customer_Id = c.Int(),
                        Invoice_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Invoices", t => t.Invoice_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Invoice_Id);
            
            CreateTable(
                "dbo.Invoices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Price = c.Double(nullable: false),
                        Unit = c.Double(nullable: false),
                        Date = c.DateTime(nullable: false, precision: 0),
                        Quantity = c.Int(nullable: false),
                        Size = c.String(unicode: false),
                        Description = c.String(unicode: false),
                        TotalPrice = c.Double(nullable: false),
                        Customer_Id = c.Int(),
                        Product_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Customers", t => t.Customer_Id)
                .ForeignKey("dbo.Products", t => t.Product_Id)
                .Index(t => t.Customer_Id)
                .Index(t => t.Product_Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(unicode: false),
                        Image = c.Binary(),
                        IsSelected = c.Boolean(nullable: false),
                        Name = c.String(unicode: false),
                        Price = c.Double(nullable: false),
                        ProductCode = c.Int(nullable: false),
                        Stock = c.Int(nullable: false),
                        Type_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Types", t => t.Type_Id)
                .Index(t => t.Type_Id);
            
            CreateTable(
                "dbo.Types",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Transactions", "Invoice_Id", "dbo.Invoices");
            DropForeignKey("dbo.Invoices", "Product_Id", "dbo.Products");
            DropForeignKey("dbo.Products", "Type_Id", "dbo.Types");
            DropForeignKey("dbo.Invoices", "Customer_Id", "dbo.Customers");
            DropForeignKey("dbo.Transactions", "Customer_Id", "dbo.Customers");
            DropIndex("dbo.Products", new[] { "Type_Id" });
            DropIndex("dbo.Invoices", new[] { "Product_Id" });
            DropIndex("dbo.Invoices", new[] { "Customer_Id" });
            DropIndex("dbo.Transactions", new[] { "Invoice_Id" });
            DropIndex("dbo.Transactions", new[] { "Customer_Id" });
            DropTable("dbo.Types");
            DropTable("dbo.Products");
            DropTable("dbo.Invoices");
            DropTable("dbo.Transactions");
            DropTable("dbo.Customers");
        }
    }
}
