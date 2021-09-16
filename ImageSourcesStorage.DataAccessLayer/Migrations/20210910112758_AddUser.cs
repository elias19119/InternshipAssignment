using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageSourcesStorage.DataAccessLayer.Migrations
{
    public partial class AddUser : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Board", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "ImageSources",
                columns: table => new
                {
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImageSources", x => x.BoardId);
                    table.ForeignKey(
                        name: "FK_ImageSources_Board_UserId",
                        column: x => x.UserId,
                        principalTable: "Boards",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pins",
                columns: table => new
                {
                    PinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pin", x => x.PinId);
                    table.ForeignKey(
                        name: "FK_Pin_Board_UserId",
                        column: x => x.UserId,
                        principalTable: "Boards",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Pin_ImageSources_BoardId",
                        column: x => x.BoardId,
                        principalTable: "ImageSources",
                        principalColumn: "BoardId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageSources_UserId",
                table: "ImageSources",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Pin_BoardId",
                table: "Pins",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_Pin_UserId",
                table: "Pins",
                column: "UserId");
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pins");

            migrationBuilder.DropTable(
                name: "ImageSources");

            migrationBuilder.DropTable(
                name: "Boards");
        }
    }
}
