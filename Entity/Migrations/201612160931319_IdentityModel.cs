namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class IdentityModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Organizations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Administrator_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.Administrator_Id, cascadeDelete: false)
                .Index(t => t.Administrator_Id);
            
            AddColumn("dbo.AspNetUsers", "Status", c => c.Int(nullable: false));
            AddColumn("dbo.AspNetUsers", "Organization_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.AspNetUsers", "Organization_Id");
            AddForeignKey("dbo.AspNetUsers", "Organization_Id", "dbo.Organizations", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Organizations", "Administrator_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUsers", "Organization_Id", "dbo.Organizations");
            DropIndex("dbo.AspNetUsers", new[] { "Organization_Id" });
            DropIndex("dbo.Organizations", new[] { "Administrator_Id" });
            DropColumn("dbo.AspNetUsers", "Organization_Id");
            DropColumn("dbo.AspNetUsers", "Status");
            DropTable("dbo.Organizations");
        }
    }
}
