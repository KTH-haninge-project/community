namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dataannotationsinmessage : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Messages", "TheMessage", c => c.String(nullable: false));
            AlterColumn("dbo.Messages", "Title", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Messages", "Title", c => c.String());
            AlterColumn("dbo.Messages", "TheMessage", c => c.String());
        }
    }
}
