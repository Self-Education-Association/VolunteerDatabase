namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class intToLong : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppUsers", "StudentNum", c => c.Long(nullable: false));
            AlterColumn("dbo.Volunteers", "StudentNum", c => c.Long(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Volunteers", "StudentNum", c => c.Int(nullable: false));
            AlterColumn("dbo.AppUsers", "StudentNum", c => c.Int(nullable: false));
        }
    }
}
