namespace eth.Eve.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EveSpace_TokenLength : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.EveSpaces", "BotApiAccessToken", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.EveSpaces", "BotApiAccessToken", c => c.String());
        }
    }
}
