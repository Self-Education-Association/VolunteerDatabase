namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AlterCreditRecordKey : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.CreditRecords");
            AlterColumn("dbo.CreditRecords", "UID", c => c.Guid(nullable: false, identity: true));
            AddPrimaryKey("dbo.CreditRecords", "UID");
        }
        
        public override void Down()
        {
            DropPrimaryKey("dbo.CreditRecords");
            AlterColumn("dbo.CreditRecords", "UID", c => c.Guid(nullable: false));
            AddPrimaryKey("dbo.CreditRecords", "UID");
        }
    }
}
