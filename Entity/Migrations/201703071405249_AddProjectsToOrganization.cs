namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectsToOrganization : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Projects", new[] { "Organization_Id" });
            AlterColumn("dbo.Projects", "Organization_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Projects", "Organization_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projects", new[] { "Organization_Id" });
            AlterColumn("dbo.Projects", "Organization_Id", c => c.Int());
            CreateIndex("dbo.Projects", "Organization_Id");
        }
    }
}
