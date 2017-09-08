namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreditRecordEdit : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CreditRecords", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.CreditRecords", new[] { "Organization_Id" });
            DropColumn("dbo.CreditRecords", "Organization_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.CreditRecords", "Organization_Id", c => c.Int());
            CreateIndex("dbo.CreditRecords", "Organization_Id");
            AddForeignKey("dbo.CreditRecords", "Organization_Id", "dbo.Organizations", "Id");
        }
    }
}
