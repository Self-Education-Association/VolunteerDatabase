namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUid : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.BlackListRecords", "Volunteer_Id", "dbo.Volunteers");
            DropForeignKey("dbo.ProjectVolunteers", "Volunteer_Id", "dbo.Volunteers");
            RenameColumn(table: "dbo.BlackListRecords", name: "Volunteer_Id", newName: "Volunteer_UID");
            RenameColumn(table: "dbo.ProjectVolunteers", name: "Volunteer_Id", newName: "Volunteer_UID");
            RenameIndex(table: "dbo.BlackListRecords", name: "IX_Volunteer_Id", newName: "IX_Volunteer_UID");
            RenameIndex(table: "dbo.ProjectVolunteers", name: "IX_Volunteer_Id", newName: "IX_Volunteer_UID");
            DropPrimaryKey("dbo.Volunteers");
            AddColumn("dbo.Volunteers", "UID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Volunteers", "Id", c => c.Int(nullable: false,identity:false));
            AddPrimaryKey("dbo.Volunteers", "UID");
            AddForeignKey("dbo.BlackListRecords", "Volunteer_UID", "dbo.Volunteers", "UID", cascadeDelete: true);
            AddForeignKey("dbo.ProjectVolunteers", "Volunteer_UID", "dbo.Volunteers", "UID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectVolunteers", "Volunteer_UID", "dbo.Volunteers");
            DropForeignKey("dbo.BlackListRecords", "Volunteer_UID", "dbo.Volunteers");
            DropPrimaryKey("dbo.Volunteers");
            AlterColumn("dbo.Volunteers", "Id", c => c.Int(nullable: false, identity: true));
            DropColumn("dbo.Volunteers", "UID");
            AddPrimaryKey("dbo.Volunteers", "Id");
            RenameIndex(table: "dbo.ProjectVolunteers", name: "IX_Volunteer_UID", newName: "IX_Volunteer_Id");
            RenameIndex(table: "dbo.BlackListRecords", name: "IX_Volunteer_UID", newName: "IX_Volunteer_Id");
            RenameColumn(table: "dbo.ProjectVolunteers", name: "Volunteer_UID", newName: "Volunteer_Id");
            RenameColumn(table: "dbo.BlackListRecords", name: "Volunteer_UID", newName: "Volunteer_Id");
            AddForeignKey("dbo.ProjectVolunteers", "Volunteer_Id", "dbo.Volunteers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.BlackListRecords", "Volunteer_Id", "dbo.Volunteers", "Id", cascadeDelete: true);
        }
    }
}
