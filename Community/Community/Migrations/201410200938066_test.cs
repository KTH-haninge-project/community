namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Groups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GroupViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AspNetUsers", "Group_Id", c => c.Int());
            CreateIndex("dbo.AspNetUsers", "Group_Id");
            AddForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.AspNetUsers", new[] { "Group_Id" });
            DropColumn("dbo.AspNetUsers", "Group_Id");
            DropTable("dbo.GroupViewModels");
            DropTable("dbo.Groups");
        }
    }
}
