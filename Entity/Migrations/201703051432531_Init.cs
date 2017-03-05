namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApprovalRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsApproved = c.Boolean(nullable: false),
                        RequestTime = c.DateTime(nullable: false),
                        ExpireTime = c.DateTime(nullable: false),
                        Note = c.String(),
                        Organization_Id = c.Int(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.AppUsers", t => t.User_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        OrganizationEnum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.BlackListRecords",
                c => new
                    {
                        UID = c.Guid(nullable: false, identity: true),
                        AddTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Volunteer_UID = c.Guid(nullable: false),
                        Adder_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                        Organization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UID)
                .ForeignKey("dbo.Volunteers", t => t.Volunteer_UID, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.Adder_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: true)
                .Index(t => t.Volunteer_UID)
                .Index(t => t.Adder_Id)
                .Index(t => t.Project_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        StudentNum = c.Int(nullable: false),
                        AccountName = c.String(),
                        Name = c.String(),
                        Salt = c.String(),
                        HashedPassword = c.String(),
                        Mobile = c.String(),
                        Email = c.String(),
                        Room = c.String(),
                        Status = c.Int(nullable: false),
                        Organization_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .Index(t => t.Organization_Id);
            
            CreateTable(
                "dbo.LogRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddTime = c.DateTime(nullable: false),
                        IsPulblic = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                        TypeNum = c.Int(nullable: false),
                        Operation = c.String(nullable: false),
                        LogContent = c.String(nullable: false),
                        Adder_Id = c.Int(nullable: false),
                        TargetAppUser_Id = c.Int(),
                        TargetProject_Id = c.Int(),
                        TargetVolunteer_UID = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.Adder_Id, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.TargetAppUser_Id)
                .ForeignKey("dbo.Projects", t => t.TargetProject_Id)
                .ForeignKey("dbo.Volunteers", t => t.TargetVolunteer_UID)
                .Index(t => t.Adder_Id)
                .Index(t => t.TargetAppUser_Id)
                .Index(t => t.TargetProject_Id)
                .Index(t => t.TargetVolunteer_UID);
            
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Place = c.String(),
                        Maximum = c.Int(nullable: false),
                        Details = c.String(),
                        Time = c.DateTime(nullable: false),
                        CreatTime = c.DateTime(),
                        Condition = c.Int(nullable: false),
                        ScoreCondition = c.Int(nullable: false),
                        Creater_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Creater_Id)
                .Index(t => t.Creater_Id);
            
            CreateTable(
                "dbo.CreditRecords",
                c => new
                    {
                        UID = c.Guid(nullable: false),
                        Score = c.Int(nullable: false),
                        Organization_Id = c.Int(),
                        Participant_UID = c.Guid(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UID)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.Volunteers", t => t.Participant_UID, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Organization_Id)
                .Index(t => t.Participant_UID)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        UID = c.Guid(nullable: false, identity: true),
                        StudentNum = c.Int(nullable: false),
                        Mobile = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Room = c.String(),
                        Class = c.String(),
                        Score = c.Int(nullable: false),
                        ProjectCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UID);
            
            CreateTable(
                "dbo.AppRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        RoleEnum = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectVolunteers",
                c => new
                    {
                        Project_Id = c.Int(nullable: false),
                        Volunteer_UID = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Volunteer_UID })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Volunteers", t => t.Volunteer_UID, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Volunteer_UID);
            
            CreateTable(
                "dbo.AppUserProjects",
                c => new
                    {
                        AppUser_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.Project_Id })
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Project_Id);
            
            CreateTable(
                "dbo.AppUserAppRoles",
                c => new
                    {
                        AppUser_Id = c.Int(nullable: false),
                        AppRole_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.AppRole_Id })
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.AppRoles", t => t.AppRole_Id, cascadeDelete: true)
                .Index(t => t.AppUser_Id)
                .Index(t => t.AppRole_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApprovalRecords", "User_Id", "dbo.AppUsers");
            DropForeignKey("dbo.ApprovalRecords", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.BlackListRecords", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.BlackListRecords", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.BlackListRecords", "Adder_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserAppUsers", "AppUser_Id1", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserAppUsers", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserAppRoles", "AppRole_Id", "dbo.AppRoles");
            DropForeignKey("dbo.AppUserAppRoles", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.AppUserProjects", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AppUsers", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.LogRecords", "TargetVolunteer_UID", "dbo.Volunteers");
            DropForeignKey("dbo.LogRecords", "TargetProject_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectVolunteers", "Volunteer_UID", "dbo.Volunteers");
            DropForeignKey("dbo.ProjectVolunteers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.CreditRecords", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.CreditRecords", "Participant_UID", "dbo.Volunteers");
            DropForeignKey("dbo.BlackListRecords", "Volunteer_UID", "dbo.Volunteers");
            DropForeignKey("dbo.CreditRecords", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.Projects", "Creater_Id", "dbo.Organizations");
            DropForeignKey("dbo.LogRecords", "TargetAppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.LogRecords", "Adder_Id", "dbo.AppUsers");
            DropIndex("dbo.AppUserAppUsers", new[] { "AppUser_Id1" });
            DropIndex("dbo.AppUserAppUsers", new[] { "AppUser_Id" });
            DropIndex("dbo.AppUserAppRoles", new[] { "AppRole_Id" });
            DropIndex("dbo.AppUserAppRoles", new[] { "AppUser_Id" });
            DropIndex("dbo.AppUserProjects", new[] { "Project_Id" });
            DropIndex("dbo.AppUserProjects", new[] { "AppUser_Id" });
            DropIndex("dbo.ProjectVolunteers", new[] { "Volunteer_UID" });
            DropIndex("dbo.ProjectVolunteers", new[] { "Project_Id" });
            DropIndex("dbo.CreditRecords", new[] { "Project_Id" });
            DropIndex("dbo.CreditRecords", new[] { "Participant_UID" });
            DropIndex("dbo.CreditRecords", new[] { "Organization_Id" });
            DropIndex("dbo.Projects", new[] { "Creater_Id" });
            DropIndex("dbo.LogRecords", new[] { "TargetVolunteer_UID" });
            DropIndex("dbo.LogRecords", new[] { "TargetProject_Id" });
            DropIndex("dbo.LogRecords", new[] { "TargetAppUser_Id" });
            DropIndex("dbo.LogRecords", new[] { "Adder_Id" });
            DropIndex("dbo.AppUsers", new[] { "Organization_Id" });
            DropIndex("dbo.BlackListRecords", new[] { "Organization_Id" });
            DropIndex("dbo.BlackListRecords", new[] { "Project_Id" });
            DropIndex("dbo.BlackListRecords", new[] { "Adder_Id" });
            DropIndex("dbo.BlackListRecords", new[] { "Volunteer_UID" });
            DropIndex("dbo.ApprovalRecords", new[] { "User_Id" });
            DropIndex("dbo.ApprovalRecords", new[] { "Organization_Id" });
            DropTable("dbo.AppUserAppUsers");
            DropTable("dbo.AppUserAppRoles");
            DropTable("dbo.AppUserProjects");
            DropTable("dbo.ProjectVolunteers");
            DropTable("dbo.AppRoles");
            DropTable("dbo.Volunteers");
            DropTable("dbo.CreditRecords");
            DropTable("dbo.Projects");
            DropTable("dbo.LogRecords");
            DropTable("dbo.AppUsers");
            DropTable("dbo.BlackListRecords");
            DropTable("dbo.Organizations");
            DropTable("dbo.ApprovalRecords");
        }
    }
}
