namespace BulkiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "Distance", c => c.Single(nullable: false));
            DropColumn("dbo.Routes", "SourceAddress");
            DropColumn("dbo.Routes", "DestinationAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Routes", "DestinationAddress", c => c.String());
            AddColumn("dbo.Routes", "SourceAddress", c => c.String());
            DropColumn("dbo.Routes", "Distance");
        }
    }
}
