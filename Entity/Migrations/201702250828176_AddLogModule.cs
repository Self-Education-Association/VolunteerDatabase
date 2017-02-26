namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddLogModule : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LogRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AddTime = c.DateTime(nullable: false),
                        IsPulblic = c.Boolean(nullable: false),
                        Type = c.Int(nullable: false),
                        TypeNum = c.Int(nullable: false),
                        Operation = c.String(nullable: false),
                        LogContent = c.String(nullable: false),
                        TargetAppUser_Id = c.Int(),
                        TargetProject_Id = c.Int(),
                        TargetVolunteer_UID = c.Guid(),
                        Adder_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.TargetAppUser_Id)
                .ForeignKey("dbo.Projects", t => t.TargetProject_Id)
                .ForeignKey("dbo.Volunteers", t => t.TargetVolunteer_UID)
                .ForeignKey("dbo.AppUsers", t => t.Adder_Id, cascadeDelete: true)
                .Index(t => t.TargetAppUser_Id)
                .Index(t => t.TargetProject_Id)
                .Index(t => t.TargetVolunteer_UID)
                .Index(t => t.Adder_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LogRecords", "Adder_Id", "dbo.AppUsers");
            DropForeignKey("dbo.LogRecords", "TargetVolunteer_UID", "dbo.Volunteers");
            DropForeignKey("dbo.LogRecords", "TargetProject_Id", "dbo.Projects");
            DropForeignKey("dbo.LogRecords", "TargetAppUser_Id", "dbo.AppUsers");
            DropIndex("dbo.LogRecords", new[] { "Adder_Id" });
            DropIndex("dbo.LogRecords", new[] { "TargetVolunteer_UID" });
            DropIndex("dbo.LogRecords", new[] { "TargetProject_Id" });
            DropIndex("dbo.LogRecords", new[] { "TargetAppUser_Id" });
            DropTable("dbo.LogRecords");
        }
    }
}
