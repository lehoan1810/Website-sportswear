namespace banhang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Item",
                c => new
                    {
                        ItemId = c.Int(nullable: false),
                        OrderId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                        ProductId = c.Int(nullable: false),
                        Product_idProduct = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ItemId, t.OrderId })
                .ForeignKey("dbo.Product", t => t.Product_idProduct)
                .ForeignKey("dbo.Order", t => t.OrderId, cascadeDelete: true)
                .Index(t => t.OrderId)
                .Index(t => t.Product_idProduct);
            
            CreateTable(
                "dbo.Product",
                c => new
                    {
                        idProduct = c.Int(nullable: false, identity: true),
                        ProductName = c.String(nullable: false),
                        Price = c.Int(nullable: false),
                        Category = c.String(),
                        SubCategory = c.String(),
                        Detail = c.String(),
                        NgayThem = c.DateTime(nullable: false),
                        Image = c.String(),
                    })
                .PrimaryKey(t => t.idProduct);
            
            CreateTable(
                "dbo.Order",
                c => new
                    {
                        OrderID = c.Int(nullable: false, identity: true),
                        Username = c.String(maxLength: 128),
                        DiaChi = c.String(),
                        GhiChu = c.String(),
                        NgayDat = c.DateTime(nullable: false),
                        TrangThai = c.String(),
                        Tong = c.Double(nullable: false),
                    })
                .PrimaryKey(t => t.OrderID)
                .ForeignKey("dbo.Users", t => t.Username)
                .Index(t => t.Username);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Username = c.String(nullable: false, maxLength: 128),
                        Password = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Lever = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Username);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Order", "Username", "dbo.Users");
            DropForeignKey("dbo.Item", "OrderId", "dbo.Order");
            DropForeignKey("dbo.Item", "Product_idProduct", "dbo.Product");
            DropIndex("dbo.Order", new[] { "Username" });
            DropIndex("dbo.Item", new[] { "Product_idProduct" });
            DropIndex("dbo.Item", new[] { "OrderId" });
            DropTable("dbo.Users");
            DropTable("dbo.Order");
            DropTable("dbo.Product");
            DropTable("dbo.Item");
        }
    }
}
