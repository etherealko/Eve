using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace eth.Eve.Storage.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EveSpaces",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BotApiAccessToken = table.Column<string>(maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EveSpaces", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PluginStoreBinaries",
                columns: table => new
                {
                    SpaceId = table.Column<long>(nullable: false),
                    PluginGuid = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    Metadata = table.Column<string>(nullable: true),
                    PluginVersion = table.Column<string>(nullable: true),
                    Value = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginStoreBinaries", x => new { x.SpaceId, x.PluginGuid, x.Key });
                    table.ForeignKey(
                        name: "FK_PluginStoreBinaries_EveSpaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "EveSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PluginStoreStrings",
                columns: table => new
                {
                    SpaceId = table.Column<long>(nullable: false),
                    PluginGuid = table.Column<Guid>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    PluginVersion = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PluginStoreStrings", x => new { x.SpaceId, x.PluginGuid, x.Key });
                    table.ForeignKey(
                        name: "FK_PluginStoreStrings_EveSpaces_SpaceId",
                        column: x => x.SpaceId,
                        principalTable: "EveSpaces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PluginStoreBinaries");

            migrationBuilder.DropTable(
                name: "PluginStoreStrings");

            migrationBuilder.DropTable(
                name: "EveSpaces");
        }
    }
}
