namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class timestampinuser : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Groups", name: "God_Id", newName: "Owner_Id");
            RenameIndex(table: "dbo.Groups", name: "IX_God_Id", newName: "IX_Owner_Id");
            AddColumn("dbo.AspNetUsers", "lastLogin", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "loginMonthCounter", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "loginMonthCounter");
            DropColumn("dbo.AspNetUsers", "lastLogin");
            RenameIndex(table: "dbo.Groups", name: "IX_Owner_Id", newName: "IX_God_Id");
            RenameColumn(table: "dbo.Groups", name: "Owner_Id", newName: "God_Id");
        }
    }
}
