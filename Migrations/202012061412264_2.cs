namespace banhang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _2 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Item", new[] { "Product_idProduct" });
            AlterColumn("dbo.Item", "Product_idProduct", c => c.Int());
            CreateIndex("dbo.Item", "Product_idProduct");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Item", new[] { "Product_idProduct" });
            AlterColumn("dbo.Item", "Product_idProduct", c => c.Int(nullable: false));
            CreateIndex("dbo.Item", "Product_idProduct");
        }
    }
}
