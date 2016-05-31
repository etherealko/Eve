namespace eth.Eve.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EveSpace_IsActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.EveSpaces", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.EveSpaces", "IsActive");
        }
    }
}
