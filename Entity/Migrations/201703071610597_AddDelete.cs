namespace VolunteerDatabase.Entity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDelete : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Projects", name: "Creater_Id", newName: "Organization_Id");
            RenameIndex(table: "dbo.Projects", name: "IX_Creater_Id", newName: "IX_Organization_Id");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Projects", name: "IX_Organization_Id", newName: "IX_Creater_Id");
            RenameColumn(table: "dbo.Projects", name: "Organization_Id", newName: "Creater_Id");
        }
    }
}
