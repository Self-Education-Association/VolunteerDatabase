namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogModule2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LogRecords", "TargetAppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.LogRecords", "TargetProject_Id", "dbo.Projects");
            DropForeignKey("dbo.LogRecords", "TargetVolunteer_UID", "dbo.Volunteers");
            DropIndex("dbo.LogRecords", new[] { "TargetProject_Id" });
            DropIndex("dbo.LogRecords", new[] { "TargetVolunteer_UID" });
            DropIndex("dbo.LogRecords", new[] { "TargetAppUser_Id" });
            AlterColumn("dbo.LogRecords", "TargetProject_Id", c => c.Int());
            AlterColumn("dbo.LogRecords", "TargetVolunteer_UID", c => c.Guid());
            AlterColumn("dbo.LogRecords", "TargetAppUser_Id", c => c.Int());
            CreateIndex("dbo.LogRecords", "TargetProject_Id");
            CreateIndex("dbo.LogRecords", "TargetVolunteer_UID");
            CreateIndex("dbo.LogRecords", "TargetAppUser_Id");
            AddForeignKey("dbo.LogRecords", "TargetAppUser_Id", "dbo.AppUsers", "Id");
            AddForeignKey("dbo.LogRecords", "TargetProject_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.LogRecords", "TargetVolunteer_UID", "dbo.Volunteers", "UID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogRecords", "TargetVolunteer_UID", "dbo.Volunteers");
            DropForeignKey("dbo.LogRecords", "TargetProject_Id", "dbo.Projects");
            DropForeignKey("dbo.LogRecords", "TargetAppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.LogRecords", new[] { "TargetAppUser_Id" });
            DropIndex("dbo.LogRecords", new[] { "TargetVolunteer_UID" });
            DropIndex("dbo.LogRecords", new[] { "TargetProject_Id" });
            AlterColumn("dbo.LogRecords", "TargetAppUser_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.LogRecords", "TargetVolunteer_UID", c => c.Guid(nullable: false));
            AlterColumn("dbo.LogRecords", "TargetProject_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.LogRecords", "TargetAppUser_Id");
            CreateIndex("dbo.LogRecords", "TargetVolunteer_UID");
            CreateIndex("dbo.LogRecords", "TargetProject_Id");
            AddForeignKey("dbo.LogRecords", "TargetVolunteer_UID", "dbo.Volunteers", "UID", cascadeDelete: true);
            AddForeignKey("dbo.LogRecords", "TargetProject_Id", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.LogRecords", "TargetAppUser_Id", "dbo.AppUsers", "Id", cascadeDelete: true);
        }
    }
}
