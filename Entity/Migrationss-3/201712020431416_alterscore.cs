namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class alterscore : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Volunteers", "Score_SrvScore");
            DropColumn("dbo.Volunteers", "Score_CmmScore");
            DropColumn("dbo.Volunteers", "Score_PncScore");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Volunteers", "Score_PncScore", c => c.Double(nullable: false));
            AddColumn("dbo.Volunteers", "Score_CmmScore", c => c.Double(nullable: false));
            AddColumn("dbo.Volunteers", "Score_SrvScore", c => c.Double(nullable: false));
        }
    }
}
