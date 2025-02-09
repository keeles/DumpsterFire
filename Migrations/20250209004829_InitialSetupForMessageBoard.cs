using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASP.NETCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetupForMessageBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Serial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Username = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Password = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ProfilePicture = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    About = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Rep = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Serial);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    Serial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserSerial = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Serial);
                    table.ForeignKey(
                        name: "FK_Boards_Users_UserSerial",
                        column: x => x.UserSerial,
                        principalTable: "Users",
                        principalColumn: "Serial",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Threads",
                columns: table => new
                {
                    Serial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserSerial = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    BoardSerial = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Threads", x => x.Serial);
                    table.ForeignKey(
                        name: "FK_Threads_Boards_BoardSerial",
                        column: x => x.BoardSerial,
                        principalTable: "Boards",
                        principalColumn: "Serial",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Threads_Users_UserSerial",
                        column: x => x.UserSerial,
                        principalTable: "Users",
                        principalColumn: "Serial",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Serial = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserSerial = table.Column<int>(type: "int", nullable: false),
                    ThreadSerial = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Serial);
                    table.ForeignKey(
                        name: "FK_Posts_Threads_ThreadSerial",
                        column: x => x.ThreadSerial,
                        principalTable: "Threads",
                        principalColumn: "Serial",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserSerial",
                        column: x => x.UserSerial,
                        principalTable: "Users",
                        principalColumn: "Serial",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_UserSerial",
                table: "Boards",
                column: "UserSerial");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_ThreadSerial",
                table: "Posts",
                column: "ThreadSerial");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserSerial",
                table: "Posts",
                column: "UserSerial");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_BoardSerial",
                table: "Threads",
                column: "BoardSerial");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_UserSerial",
                table: "Threads",
                column: "UserSerial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Threads");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
