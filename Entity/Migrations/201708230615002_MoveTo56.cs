namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveTo56 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.CreditRecords", "Score");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreditRecords", "Score", c => c.Double(nullable: false));
        }
    }
}
