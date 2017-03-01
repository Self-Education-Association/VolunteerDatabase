namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddStuNumInAppUser : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "StudentNum", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "StudentNum");
        }
    }
}
