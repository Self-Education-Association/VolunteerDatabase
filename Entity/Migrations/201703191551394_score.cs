namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class score : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CreditRecords", "Score", c => c.Double(nullable: false));
            AlterColumn("dbo.Volunteers", "Score", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Volunteers", "Score", c => c.Int(nullable: false));
            AlterColumn("dbo.CreditRecords", "Score", c => c.Int(nullable: false));
        }
    }
}
