namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeStudentIDtoString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppUsers", "StudentNum", c => c.String());
            AlterColumn("dbo.Volunteers", "StudentNum", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Volunteers", "StudentNum", c => c.Long(nullable: false));
            AlterColumn("dbo.AppUsers", "StudentNum", c => c.Long(nullable: false));
        }
    }
}
