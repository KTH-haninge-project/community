namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "ApplicationUser_Id" });
            AddColumn("dbo.Messages", "sendTimeStamp", c => c.DateTime(nullable: false));
            AddColumn("dbo.ReadEntries", "Active", c => c.Boolean(nullable: false));
            DropColumn("dbo.Messages", "ApplicationUser_Id");
            DropTable("dbo.MessageViewModels");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.MessageViewModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Deleted = c.Boolean(nullable: false),
                        Read = c.String(),
                        Title = c.String(),
                        Receiver = c.String(),
                        TheMessage = c.String(),
                        Sender = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Messages", "ApplicationUser_Id", c => c.String(maxLength: 128));
            DropColumn("dbo.ReadEntries", "Active");
            DropColumn("dbo.Messages", "sendTimeStamp");
            CreateIndex("dbo.Messages", "ApplicationUser_Id");
            AddForeignKey("dbo.Messages", "ApplicationUser_Id", "dbo.AspNetUsers", "Id");
        }
    }
}
