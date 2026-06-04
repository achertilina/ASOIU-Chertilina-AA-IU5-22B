using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Homework3.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Databases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    SizeGb = table.Column<int>(type: "INTEGER", nullable: false),
                    ServerId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Databases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Databases_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Servers",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Oracle Enterprise" },
                    { 2, "Microsoft SQL Server" },
                    { 3, "PostgreSQL" },
                    { 4, "MongoDB Atlas" },
                    { 5, "IBM Db2" }
                });

            migrationBuilder.InsertData(
                table: "Databases",
                columns: new[] { "Id", "Name", "ServerId", "SizeGb" },
                values: new object[,]
                {
                    { 1, "CRM_Prod", 1, 512 },
                    { 2, "ERP_Finance", 1, 1024 },
                    { 3, "Analytics_Dev", 2, 256 },
                    { 4, "Reporting", 2, 128 },
                    { 5, "MainDB", 3, 64 },
                    { 6, "BackupDB", 3, 32 },
                    { 7, "UserData", 4, 512 },
                    { 8, "Logs", 4, 256 },
                    { 9, "Warehouse", 5, 2048 },
                    { 10, "Staging", 5, 1024 },
                    { 11, "TestEnv", 1, 128 },
                    { 12, "Archive", 3, 16 },
                    { 13, "TempDB", 2, 64 },
                    { 14, "ConfigDB", 4, 32 },
                    { 15, "Metabase", 5, 512 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Databases_ServerId",
                table: "Databases",
                column: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Databases");

            migrationBuilder.DropTable(
                name: "Servers");
        }
    }
}
