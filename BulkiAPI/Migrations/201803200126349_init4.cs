namespace BulkiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init4 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Routes", "SourceAddress", c => c.String(nullable: false, maxLength: 100));
            AlterColumn("dbo.Routes", "DestinationAddress", c => c.String(nullable: false, maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Routes", "DestinationAddress", c => c.String());
            AlterColumn("dbo.Routes", "SourceAddress", c => c.String());
        }
    }
}
