namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteCascedeOnDeleteOfBlackRecord : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlackListRecords", "Organization_Id", "dbo.Organizations");
            AddForeignKey("dbo.BlackListRecords", "Organization_Id", "dbo.Organizations", "Id", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BlackListRecords", "Organization_Id", "dbo.Organizations");
            AddForeignKey("dbo.BlackListRecords", "Organization_Id", "dbo.Organizations", "Id",cascadeDelete:true);
        }
    }
}
