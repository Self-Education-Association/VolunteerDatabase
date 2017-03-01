namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ApprovalRecordPUpdate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApprovalRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IsApproved = c.Boolean(nullable: false),
                        RequestTime = c.DateTime(nullable: false),
                        ExpireTime = c.DateTime(nullable: false),
                        Note = c.String(),
                        User_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AppUsers", t => t.User_Id)
                .Index(t => t.User_Id);
            
            AlterColumn("dbo.Projects", "Time", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ApprovalRecords", "User_Id", "dbo.AppUsers");
            DropIndex("dbo.ApprovalRecords", new[] { "User_Id" });
            AlterColumn("dbo.Projects", "Time", c => c.DateTime());
            DropTable("dbo.ApprovalRecords");
        }
    }
}
