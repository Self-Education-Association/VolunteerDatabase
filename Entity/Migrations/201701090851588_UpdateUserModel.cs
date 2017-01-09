namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AppUsers", "AccountName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AppUsers", "AccountName");
        }
    }
}
