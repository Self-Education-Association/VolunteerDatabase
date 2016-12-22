namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateIdentityModel : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.AspNetUsers", newName: "AppUsers");
            RenameTable(name: "dbo.AspNetRoles", newName: "AppRoles");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Organizations", "Administrator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Organizations", new[] { "Administrator_Id" });
            DropIndex("dbo.AppUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AppRoles", "RoleNameIndex");
            DropPrimaryKey("dbo.AppUsers");
            DropPrimaryKey("dbo.AppRoles");
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
            
            AddColumn("dbo.AppUsers", "Name", c => c.String());
            AddColumn("dbo.AppUsers", "Salt", c => c.String());
            AddColumn("dbo.AppUsers", "HashedPassword", c => c.String());
            AddColumn("dbo.AppUsers", "Mobile", c => c.String());
            AddColumn("dbo.AppUsers", "Room", c => c.String());
            AddColumn("dbo.AppRoles", "RoleEnum", c => c.Int(nullable: false));
            AlterColumn("dbo.Organizations", "Administrator_Id", c => c.Int(nullable: false));
            AlterColumn("dbo.AppUsers", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AppUsers", "Email", c => c.String());
            AlterColumn("dbo.AppRoles", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.AppRoles", "Name", c => c.String());
            AddPrimaryKey("dbo.AppUsers", "Id");
            AddPrimaryKey("dbo.AppRoles", "Id");
            CreateIndex("dbo.Organizations", "Administrator_Id");
            AddForeignKey("dbo.Organizations", "Administrator_Id", "dbo.AppUsers", "Id", cascadeDelete: false);
            DropColumn("dbo.AppUsers", "EmailConfirmed");
            DropColumn("dbo.AppUsers", "PasswordHash");
            DropColumn("dbo.AppUsers", "SecurityStamp");
            DropColumn("dbo.AppUsers", "PhoneNumber");
            DropColumn("dbo.AppUsers", "PhoneNumberConfirmed");
            DropColumn("dbo.AppUsers", "TwoFactorEnabled");
            DropColumn("dbo.AppUsers", "LockoutEndDateUtc");
            DropColumn("dbo.AppUsers", "LockoutEnabled");
            DropColumn("dbo.AppUsers", "AccessFailedCount");
            DropColumn("dbo.AppUsers", "UserName");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserRoles");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId });
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId });
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.AppUsers", "UserName", c => c.String(nullable: false, maxLength: 256));
            AddColumn("dbo.AppUsers", "AccessFailedCount", c => c.Int(nullable: false));
            AddColumn("dbo.AppUsers", "LockoutEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUsers", "LockoutEndDateUtc", c => c.DateTime());
            AddColumn("dbo.AppUsers", "TwoFactorEnabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUsers", "PhoneNumberConfirmed", c => c.Boolean(nullable: false));
            AddColumn("dbo.AppUsers", "PhoneNumber", c => c.String());
            AddColumn("dbo.AppUsers", "SecurityStamp", c => c.String());
            AddColumn("dbo.AppUsers", "PasswordHash", c => c.String());
            AddColumn("dbo.AppUsers", "EmailConfirmed", c => c.Boolean(nullable: false));
            DropForeignKey("dbo.Organizations", "Administrator_Id", "dbo.AppUsers");
            DropForeignKey("dbo.AppUserAppRoles", "AppRole_Id", "dbo.AppRoles");
            DropForeignKey("dbo.AppUserAppRoles", "AppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.AppUserAppRoles", new[] { "AppRole_Id" });
            DropIndex("dbo.AppUserAppRoles", new[] { "AppUser_Id" });
            DropIndex("dbo.Organizations", new[] { "Administrator_Id" });
            DropPrimaryKey("dbo.AppRoles");
            DropPrimaryKey("dbo.AppUsers");
            AlterColumn("dbo.AppRoles", "Name", c => c.String(nullable: false, maxLength: 256));
            AlterColumn("dbo.AppRoles", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.AppUsers", "Email", c => c.String(maxLength: 256));
            AlterColumn("dbo.AppUsers", "Id", c => c.String(nullable: false, maxLength: 128));
            AlterColumn("dbo.Organizations", "Administrator_Id", c => c.String(nullable: false, maxLength: 128));
            DropColumn("dbo.AppRoles", "RoleEnum");
            DropColumn("dbo.AppUsers", "Room");
            DropColumn("dbo.AppUsers", "Mobile");
            DropColumn("dbo.AppUsers", "HashedPassword");
            DropColumn("dbo.AppUsers", "Salt");
            DropColumn("dbo.AppUsers", "Name");
            DropTable("dbo.AppUserAppRoles");
            AddPrimaryKey("dbo.AppRoles", "Id");
            AddPrimaryKey("dbo.AppUsers", "Id");
            CreateIndex("dbo.AppRoles", "Name", unique: true, name: "RoleNameIndex");
            CreateIndex("dbo.AspNetUserRoles", "RoleId");
            CreateIndex("dbo.AspNetUserRoles", "UserId");
            CreateIndex("dbo.AspNetUserLogins", "UserId");
            CreateIndex("dbo.AspNetUserClaims", "UserId");
            CreateIndex("dbo.AppUsers", "UserName", unique: true, name: "UserNameIndex");
            CreateIndex("dbo.Organizations", "Administrator_Id");
            AddForeignKey("dbo.Organizations", "Administrator_Id", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            AddForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
            RenameTable(name: "dbo.AppRoles", newName: "AspNetRoles");
            RenameTable(name: "dbo.AppUsers", newName: "AspNetUsers");
        }
    }
}
