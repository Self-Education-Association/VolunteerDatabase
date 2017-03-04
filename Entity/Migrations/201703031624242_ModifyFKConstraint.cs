namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifyFKConstraint : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.LogRecords", "Adder_Id", "dbo.AppUsers");
            DropForeignKey("dbo.CreditRecords", "Participant_UID", "dbo.Volunteers");
            DropForeignKey("dbo.CreditRecords", "Project_Id", "dbo.Projects");
            DropIndex("dbo.LogRecords", new[] { "Adder_Id" });
            DropIndex("dbo.CreditRecords", new[] { "Participant_UID" });
            DropIndex("dbo.CreditRecords", new[] { "Project_Id" });
            AlterColumn("dbo.LogRecords", "Adder_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.CreditRecords", "Participant_UID", c => c.Guid(nullable: false));
            AlterColumn("dbo.CreditRecords", "Project_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.LogRecords", "Adder_Id");
            CreateIndex("dbo.CreditRecords", "Participant_UID");
            CreateIndex("dbo.CreditRecords", "Project_Id");
            AddForeignKey("dbo.LogRecords", "Adder_Id", "dbo.AppUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CreditRecords", "Participant_UID", "dbo.Volunteers", "UID", cascadeDelete: true);
            AddForeignKey("dbo.CreditRecords", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditRecords", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.CreditRecords", "Participant_UID", "dbo.Volunteers");
            DropForeignKey("dbo.LogRecords", "Adder_Id", "dbo.AppUsers");
            DropIndex("dbo.CreditRecords", new[] { "Project_Id" });
            DropIndex("dbo.CreditRecords", new[] { "Participant_UID" });
            DropIndex("dbo.LogRecords", new[] { "Adder_Id" });
            AlterColumn("dbo.CreditRecords", "Project_Id", c => c.Int());
            AlterColumn("dbo.CreditRecords", "Participant_UID", c => c.Guid());
            AlterColumn("dbo.LogRecords", "Adder_Id", c => c.Int());
            CreateIndex("dbo.CreditRecords", "Project_Id");
            CreateIndex("dbo.CreditRecords", "Participant_UID");
            CreateIndex("dbo.LogRecords", "Adder_Id");
            AddForeignKey("dbo.CreditRecords", "Project_Id", "dbo.Projects", "Id");
            AddForeignKey("dbo.CreditRecords", "Participant_UID", "dbo.Volunteers", "UID");
            AddForeignKey("dbo.LogRecords", "Adder_Id", "dbo.AppUsers", "Id");
        }
    }
}
