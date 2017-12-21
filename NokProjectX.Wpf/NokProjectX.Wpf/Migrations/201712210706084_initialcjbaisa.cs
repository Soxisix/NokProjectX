namespace NokProjectX.Wpf.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initialcjbaisa : DbMigration
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
            DropForeignKey("dbo.Products", "Type_Id", "dbo.Types");
            DropIndex("dbo.Products", new[] { "Type_Id" });
            DropTable("dbo.Types");
            DropTable("dbo.Products");
            DropTable("dbo.Customers");
        }
    }
}
