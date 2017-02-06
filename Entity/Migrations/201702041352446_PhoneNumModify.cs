namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PhoneNumModify : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Volunteers", "PhoneNum");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Volunteers", "PhoneNum", c => c.Int(nullable: false));
        }
    }
}
