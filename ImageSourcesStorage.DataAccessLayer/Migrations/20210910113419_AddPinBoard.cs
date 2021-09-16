using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImageSourcesStorage.DataAccessLayer.Migrations
{
    public partial class AddPinBoard : Migration
    {
        /// <inheritdoc/>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pin_Board_UserId",
                table: "Pins");

            migrationBuilder.DropForeignKey(
                name: "FK_Pin_ImageSources_BoardId",
                table: "Pins");

            migrationBuilder.DropTable(
                name: "ImageSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Board",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Boards");

            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "Boards",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Board",
                table: "Boards",
                column: "BoardId");

            migrationBuilder.CreateTable(
                name: "PinBoards",
                columns: table => new
                {
                    PinBoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PinId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PinBoards", x => x.PinBoardId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Board_UserId",
                table: "Boards",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_User_UserId",
                table: "Boards",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pin_Board_BoardId",
                table: "Pins",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pin_User_UserId",
                table: "Pins",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_User_UserId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Pin_Board_BoardId",
                table: "Pins");

            migrationBuilder.DropForeignKey(
                name: "FK_Pin_User_UserId",
                table: "Pins");

            migrationBuilder.DropTable(
                name: "PinBoards");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Board",
                table: "Boards");

            migrationBuilder.DropIndex(
                name: "IX_Board_UserId",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Boards");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Boards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Board",
                table: "Boards",
                column: "UserId");

            migrationBuilder.CreateTable(
                name: "ImageSources",
                columns: table => new
                {
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_ImageSources_UserId",
                table: "ImageSources",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pin_Board_UserId",
                table: "Pins",
                column: "UserId",
                principalTable: "Boards",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pin_ImageSources_BoardId",
                table: "Pins",
                column: "BoardId",
                principalTable: "ImageSources",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
