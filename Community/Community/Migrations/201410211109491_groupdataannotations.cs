namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class groupdataannotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Groups", "Name", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Groups", "Name", c => c.String());
        }
    }
}
