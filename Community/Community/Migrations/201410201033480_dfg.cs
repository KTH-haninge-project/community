namespace Community.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dfg : DbMigration
    {
        public override void Up()
        {
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
            
        }
    }
}
