namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sdfd : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Groups", "God_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Groups", "God_Id");
            AddForeignKey("dbo.Groups", "God_Id", "dbo.AspNetUsers", "Id");
            DropTable("dbo.GroupViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.GroupViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.Groups", "God_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Groups", new[] { "God_Id" });
            DropColumn("dbo.Groups", "God_Id");
        }
    }
}
