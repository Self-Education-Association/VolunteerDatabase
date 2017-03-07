namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectsToOrganization : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Projects", new[] { "Creater_Id" });
            AlterColumn("dbo.Projects", "Creater_Id", c => c.Int(nullable: false));
            CreateIndex("dbo.Projects", "Creater_Id");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Projects", new[] { "Creater_Id" });
            AlterColumn("dbo.Projects", "Creater_Id", c => c.Int());
            CreateIndex("dbo.Projects", "Creater_Id");
        }
    }
}
