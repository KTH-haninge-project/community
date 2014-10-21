namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class test3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups");
            DropIndex("dbo.AspNetUsers", new[] { "Group_Id" });
            RenameColumn(table: "dbo.Groups", name: "God_Id", newName: "Owner_Id");
            RenameIndex(table: "dbo.Groups", name: "IX_God_Id", newName: "IX_Owner_Id");
            CreateTable(
                "dbo.T_GROUP_USER",
                c => new
                    {
                        ApplicationUser_id = c.String(nullable: false, maxLength: 128),
                        Group_id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ApplicationUser_id, t.Group_id })
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUser_id, cascadeDelete: true)
                .ForeignKey("dbo.Groups", t => t.Group_id, cascadeDelete: true)
                .Index(t => t.ApplicationUser_id)
                .Index(t => t.Group_id);
            
            AddColumn("dbo.AspNetUsers", "lastLogin", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "loginMonthCounter", c => c.Int(nullable: false));
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Messages", "TheMessage", c => c.String(nullable: false));
            AlterColumn("dbo.Messages", "Title", c => c.String(nullable: false, maxLength: 100));
            DropColumn("dbo.AspNetUsers", "Group_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Group_Id", c => c.Int());
            DropForeignKey("dbo.T_GROUP_USER", "Group_id", "dbo.Groups");
            DropForeignKey("dbo.T_GROUP_USER", "ApplicationUser_id", "dbo.AspNetUsers");
            DropIndex("dbo.T_GROUP_USER", new[] { "Group_id" });
            DropIndex("dbo.T_GROUP_USER", new[] { "ApplicationUser_id" });
            AlterColumn("dbo.Messages", "Title", c => c.String());
            AlterColumn("dbo.Messages", "TheMessage", c => c.String());
            AlterColumn("dbo.Groups", "Name", c => c.String());
            DropColumn("dbo.AspNetUsers", "loginMonthCounter");
            DropColumn("dbo.AspNetUsers", "lastLogin");
            DropTable("dbo.T_GROUP_USER");
            RenameIndex(table: "dbo.Groups", name: "IX_Owner_Id", newName: "IX_God_Id");
            RenameColumn(table: "dbo.Groups", name: "Owner_Id", newName: "God_Id");
            CreateIndex("dbo.AspNetUsers", "Group_Id");
            AddForeignKey("dbo.AspNetUsers", "Group_Id", "dbo.Groups", "Id");
        }
    }
}
