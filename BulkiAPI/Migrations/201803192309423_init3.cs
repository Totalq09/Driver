namespace BulkiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Routes", "SourceAddress", c => c.String());
            AddColumn("dbo.Routes", "DestinationAddress", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Routes", "DestinationAddress");
            DropColumn("dbo.Routes", "SourceAddress");
        }
    }
}
