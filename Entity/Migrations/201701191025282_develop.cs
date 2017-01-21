namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class develop : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Place = c.String(),
                        Maximum = c.Int(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        CreatTime = c.DateTime(nullable: false),
                        Condition = c.Int(nullable: false),
                        Creater_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Organizations", t => t.Creater_Id)
                .Index(t => t.Creater_Id);
            
            CreateTable(
                "dbo.Volunteers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Room = c.String(),
                        Score = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ProjectVolunteers",
                c => new
                    {
                        Project_Id = c.String(nullable: false, maxLength: 128),
                        Volunteer_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Project_Id, t.Volunteer_Id })
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .ForeignKey("dbo.Volunteers", t => t.Volunteer_Id, cascadeDelete: true)
                .Index(t => t.Project_Id)
                .Index(t => t.Volunteer_Id);
            
            CreateTable(
                "dbo.AppUserProjects",
                c => new
                    {
                        AppUser_Id = c.Int(nullable: false),
                        Project_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.AppUser_Id, t.Project_Id })
                .ForeignKey("dbo.AppUsers", t => t.AppUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.Projects", t => t.Project_Id, cascadeDelete: true)
                .Index(t => t.AppUser_Id)
                .Index(t => t.Project_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUserProjects", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.AppUserProjects", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.ProjectVolunteers", "Volunteer_Id", "dbo.Volunteers");
            DropForeignKey("dbo.ProjectVolunteers", "Project_Id", "dbo.Projects");
            DropForeignKey("dbo.Projects", "Creater_Id", "dbo.Organizations");
            DropIndex("dbo.AppUserProjects", new[] { "Project_Id" });
            DropIndex("dbo.AppUserProjects", new[] { "AppUser_Id" });
            DropIndex("dbo.ProjectVolunteers", new[] { "Volunteer_Id" });
            DropIndex("dbo.ProjectVolunteers", new[] { "Project_Id" });
            DropIndex("dbo.Projects", new[] { "Creater_Id" });
            DropTable("dbo.AppUserProjects");
            DropTable("dbo.ProjectVolunteers");
            DropTable("dbo.Volunteers");
            DropTable("dbo.Projects");
        }
    }
}
