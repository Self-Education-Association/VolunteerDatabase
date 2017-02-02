namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Test : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProjectVolunteers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.AppUserProjects", "Project_Id", "dbo.Projects");
            DropIndex("dbo.ProjectVolunteers", new[] { "Project_Id" });
            DropIndex("dbo.AppUserProjects", new[] { "Project_Id" });
            DropPrimaryKey("dbo.Projects");
            DropPrimaryKey("dbo.AppUserProjects");
            DropPrimaryKey("dbo.ProjectVolunteers");
            CreateTable(
                "dbo.BlackListRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        Status = c.Int(nullable: false),
                        Volunteer_Id = c.Int(nullable: false),
                        Adder_Id = c.Int(nullable: false),
                        Organization_Id = c.Int(nullable: false),
                        Project_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Volunteers", t => t.Volunteer_Id, cascadeDelete: true)
                .ForeignKey("dbo.AppUsers", t => t.Adder_Id, cascadeDelete: true)
                .ForeignKey("dbo.Organizations", t => t.Organization_Id)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.Volunteer_Id)
                .Index(t => t.Adder_Id)
                .Index(t => t.Organization_Id)
                .Index(t => t.Project_Id);
            
            AddColumn("dbo.Projects", "Details", c => c.String());
            AddColumn("dbo.Projects", "Time", c => c.DateTime());
            AddColumn("dbo.Projects", "ScoreCondition", c => c.Int(nullable: false));
            AddColumn("dbo.Volunteers", "PhoneNum", c => c.Int(nullable: false));
            AddColumn("dbo.Volunteers", "Mobile", c => c.String());
            AddColumn("dbo.Volunteers", "Class", c => c.String());
            AlterColumn("dbo.Projects", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Projects", "CreatTime", c => c.DateTime());
            AlterColumn("dbo.AppUserProjects", "Project_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.ProjectVolunteers", "Project_Id", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Projects", "Id");
            AddPrimaryKey("dbo.AppUserProjects", new[] { "AppUser_Id", "Project_Id" });
            AddPrimaryKey("dbo.ProjectVolunteers", new[] { "Project_Id", "Volunteer_Id" });
            CreateIndex("dbo.ProjectVolunteers", "Project_Id");
            CreateIndex("dbo.AppUserProjects", "Project_Id");
            AddForeignKey("dbo.ProjectVolunteers", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AppUserProjects", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
            DropColumn("dbo.Projects", "StartTime");
            DropColumn("dbo.Projects", "EndTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "EndTime", c => c.DateTime(nullable: false));
            AddColumn("dbo.Projects", "StartTime", c => c.DateTime(nullable: false));
            DropForeignKey("dbo.AppUserProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.ProjectVolunteers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.BlackListRecords", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.BlackListRecords", "Organization_Id", "dbo.Organizations");
            DropForeignKey("dbo.BlackListRecords", "Adder_Id", "dbo.AppUsers");
            DropForeignKey("dbo.BlackListRecords", "Volunteer_Id", "dbo.Volunteers");
            DropIndex("dbo.AppUserProjects", new[] { "Project_Id" });
            DropIndex("dbo.ProjectVolunteers", new[] { "Project_Id" });
            DropIndex("dbo.BlackListRecords", new[] { "Project_Id" });
            DropIndex("dbo.BlackListRecords", new[] { "Organization_Id" });
            DropIndex("dbo.BlackListRecords", new[] { "Adder_Id" });
            DropIndex("dbo.BlackListRecords", new[] { "Volunteer_Id" });
            DropPrimaryKey("dbo.ProjectVolunteers");
            DropPrimaryKey("dbo.AppUserProjects");
            DropPrimaryKey("dbo.Projects");
            AlterColumn("dbo.ProjectVolunteers", "Project_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AppUserProjects", "Project_Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Projects", "CreatTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Projects", "Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.Volunteers", "Class");
            DropColumn("dbo.Volunteers", "Mobile");
            DropColumn("dbo.Volunteers", "PhoneNum");
            DropColumn("dbo.Projects", "ScoreCondition");
            DropColumn("dbo.Projects", "Time");
            DropColumn("dbo.Projects", "Details");
            DropTable("dbo.BlackListRecords");
            AddPrimaryKey("dbo.ProjectVolunteers", new[] { "Project_Id", "Volunteer_Id" });
            AddPrimaryKey("dbo.AppUserProjects", new[] { "AppUser_Id", "Project_Id" });
            AddPrimaryKey("dbo.Projects", "Id");
            CreateIndex("dbo.AppUserProjects", "Project_Id");
            CreateIndex("dbo.ProjectVolunteers", "Project_Id");
            AddForeignKey("dbo.AppUserProjects", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
            AddForeignKey("dbo.ProjectVolunteers", "Project_Id", "dbo.Projects", "Id", cascadeDelete: true);
        }
    }
}
