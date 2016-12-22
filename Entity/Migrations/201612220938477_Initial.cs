namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AppUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
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
                .ForeignKey("dbo.Organizations", t => t.Organization_Id, cascadeDelete: true)
                .Index(t => t.Organization_Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AppUserAppRoles", "AppRole_Id", "dbo.AppRoles");
            DropForeignKey("dbo.AppUserAppRoles", "AppUser_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AppUsers", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.AppUserAppRoles", new[] { "AppRole_Id" });
            DropIndex("dbo.AppUserAppRoles", new[] { "AppUser_Id" });
            DropIndex("dbo.AppUsers", new[] { "Organization_Id" });
            DropTable("dbo.AppUserAppRoles");
            DropTable("dbo.AppRoles");
            DropTable("dbo.AppUsers");
            DropTable("dbo.Organizations");
        }
    }
}
