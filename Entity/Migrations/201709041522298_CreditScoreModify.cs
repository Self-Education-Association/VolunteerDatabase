namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditScoreModify : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Volunteers", "Score");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Volunteers", "Score", c => c.Double(nullable: false));
        }
    }
}
