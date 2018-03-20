namespace BulkiAPI.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v01 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Routes", "Distance", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Routes", "Distance", c => c.Single(nullable: false));
        }
    }
}
