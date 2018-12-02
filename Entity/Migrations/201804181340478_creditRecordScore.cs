namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creditRecordScore : DbMigration
    {
        public override void Up()
        {
            //DropColumn("dbo.CreditRecords", "PncScore");
            //DropColumn("dbo.CreditRecords", "SrvScore");
            //DropColumn("dbo.CreditRecords", "CmmScore");
        }

        public override void Down()
        {
            //AddColumn("dbo.CreditRecords", "CmmScore", c => c.Int(nullable: false));
            //AddColumn("dbo.CreditRecords", "SrvScore", c => c.Int(nullable: false));
            //AddColumn("dbo.CreditRecords", "PncScore", c => c.Int(nullable: false));
        }
    }
}
