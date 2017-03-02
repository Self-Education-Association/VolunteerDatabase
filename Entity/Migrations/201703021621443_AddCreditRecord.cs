namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCreditRecord : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CreditRecords",
                c => new
                    {
                        UID = c.Guid(nullable: false),
                        Score = c.Int(nullable: false),
                        Organization_Id = c.Int(),
                        Participant_UID = c.Guid(),
                        Project_Id = c.Int(),
                    })
                .PrimaryKey(t => t.UID)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.Volunteers", t => t.Participant_UID)
                .ForeignKey("dbo.Projects", t => t.Project_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.Participant_UID)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.AppUserAppUsers",
                c => new
                    {
                        AppUser_Id = c.Int(nullable: false),
                        AppUser_Id1 = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.AppUser_Id1 })
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id)
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id1)
                .Index(t => t.AppUser_Id)
                .Index(t => t.AppUser_Id1);
            
            AddColumn("dbo.ApprovalRecords", "Organization_Id", c => c.Int());
            AddColumn("dbo.Volunteers", "ProjectCount", c => c.Int(nullable: false));
            AlterColumn("dbo.AppUsers", "StudentNum", c => c.Int());
            CreateIndex("dbo.ApprovalRecords", "Organization_Id");
            AddForeignKey("dbo.ApprovalRecords", "Organization_Id", "dbo.Organizations", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.CreditRecords", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.CreditRecords", "Participant_UID", "dbo.Volunteers");
            DropForeignKey("dbo.CreditRecords", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.ApprovalRecords", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.AppUserAppUsers", "AppUser_Id1", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserAppUsers", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.AppUserAppUsers", new[] { "AppUser_Id1" });
            DropIndex("dbo.AppUserAppUsers", new[] { "AppUser_Id" });
            DropIndex("dbo.CreditRecords", new[] { "Project_Id" });
            DropIndex("dbo.CreditRecords", new[] { "Participant_UID" });
            DropIndex("dbo.CreditRecords", new[] { "Organization_Id" });
            DropIndex("dbo.ApprovalRecords", new[] { "Organization_Id" });
            AlterColumn("dbo.AppUsers", "StudentNum", c => c.String());
            DropColumn("dbo.Volunteers", "ProjectCount");
            DropColumn("dbo.ApprovalRecords", "Organization_Id");
            DropTable("dbo.AppUserAppUsers");
            DropTable("dbo.CreditRecords");
        }
    }
}
