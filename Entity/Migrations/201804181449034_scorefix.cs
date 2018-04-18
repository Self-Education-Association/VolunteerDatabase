namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class scorefix : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.CreditRecords", "Score_UID", c => c.Guid());
            CreateIndex("dbo.CreditRecords", "Score_UID");
            AddForeignKey("dbo.CreditRecords", "Score_UID", "dbo.CreditRecords", "UID");
            DropColumn("dbo.CreditRecords", "Score_PncScore");
            DropColumn("dbo.CreditRecords", "Score_SrvScore");
            DropColumn("dbo.CreditRecords", "Score_CmmScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreditRecords", "Score_CmmScore", c => c.Int(nullable: false));
            AddColumn("dbo.CreditRecords", "Score_SrvScore", c => c.Int(nullable: false));
            AddColumn("dbo.CreditRecords", "Score_PncScore", c => c.Int(nullable: false));
            DropForeignKey("dbo.CreditRecords", "Score_UID", "dbo.CreditRecords");
            DropIndex("dbo.CreditRecords", new[] { "Score_UID" });
            DropColumn("dbo.CreditRecords", "Score_UID");
        }
    }
}
