namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StructToClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Volunteers", "Score_SrvScore", c => c.Double(nullable: false));
            AddColumn("dbo.Volunteers", "Score_CmmScore", c => c.Double(nullable: false));
            AddColumn("dbo.Volunteers", "Score_PncScore", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Volunteers", "Score_PncScore");
            DropColumn("dbo.Volunteers", "Score_CmmScore");
            DropColumn("dbo.Volunteers", "Score_SrvScore");
        }
    }
}
