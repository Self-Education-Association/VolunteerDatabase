namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateOrganizationModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Organizations", "OrganizationEnum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Organizations", "OrganizationEnum");
        }
    }
}
