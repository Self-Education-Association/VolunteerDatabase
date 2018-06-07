namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditRecords", "PncScore", c => c.Int(nullable: false));
            AddColumn("dbo.CreditRecords", "SrvScore", c => c.Int(nullable: false));
            AddColumn("dbo.CreditRecords", "CmmScore", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.CreditRecords", "CmmScore");
            DropColumn("dbo.CreditRecords", "SrvScore");
            DropColumn("dbo.CreditRecords", "PncScore");
        }
    }
}
