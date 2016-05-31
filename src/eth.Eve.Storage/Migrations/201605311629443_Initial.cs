namespace eth.Eve.Storage.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.EveSpaces",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BotApiAccessToken = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PluginStoreBinaries",
                c => new
                    {
                        SpaceId = c.Long(nullable: false),
                        PluginGuid = c.Guid(nullable: false),
                        Key = c.String(nullable: false, maxLength: 128),
                        PluginVersion = c.String(),
                        Metadata = c.String(),
                        Value = c.Binary(nullable: false),
                    })
                .PrimaryKey(t => new { t.SpaceId, t.PluginGuid, t.Key })
                .ForeignKey("dbo.EveSpaces", t => t.SpaceId, cascadeDelete: true)
                .Index(t => t.SpaceId);
            
            CreateTable(
                "dbo.PluginStoreStrings",
                c => new
                    {
                        SpaceId = c.Long(nullable: false),
                        PluginGuid = c.Guid(nullable: false),
                        Key = c.String(nullable: false, maxLength: 128),
                        PluginVersion = c.String(),
                        Value = c.String(),
                    })
                .PrimaryKey(t => new { t.SpaceId, t.PluginGuid, t.Key })
                .ForeignKey("dbo.EveSpaces", t => t.SpaceId, cascadeDelete: true)
                .Index(t => t.SpaceId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PluginStoreStrings", "SpaceId", "dbo.EveSpaces");
            DropForeignKey("dbo.PluginStoreBinaries", "SpaceId", "dbo.EveSpaces");
            DropIndex("dbo.PluginStoreStrings", new[] { "SpaceId" });
            DropIndex("dbo.PluginStoreBinaries", new[] { "SpaceId" });
            DropTable("dbo.PluginStoreStrings");
            DropTable("dbo.PluginStoreBinaries");
            DropTable("dbo.EveSpaces");
        }
    }
}
