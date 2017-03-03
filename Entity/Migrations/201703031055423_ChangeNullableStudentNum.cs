namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeNullableStudentNum : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AppUsers", "StudentNum", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AppUsers", "StudentNum", c => c.Int());
        }
    }
}
