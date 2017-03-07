namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSkillColumn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Volunteers", "Skill", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Volunteers", "Skill");
        }
    }
}
