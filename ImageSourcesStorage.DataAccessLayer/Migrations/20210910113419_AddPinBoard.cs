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
                table: "Pin");

            migrationBuilder.DropForeignKey(
                name: "FK_Pin_ImageSources_BoardId",
                table: "Pin");

            migrationBuilder.DropTable(
                name: "ImageSources");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Board",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Board");

            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "Board",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Board",
                table: "Board",
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
                name: "User",
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
                table: "Board",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Board_User_UserId",
                table: "Board",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pin_Board_BoardId",
                table: "Pin",
                column: "BoardId",
                principalTable: "Board",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Pin_User_UserId",
                table: "Pin",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc/>
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Board_User_UserId",
                table: "Board");

            migrationBuilder.DropForeignKey(
                name: "FK_Pin_Board_BoardId",
                table: "Pin");

            migrationBuilder.DropForeignKey(
                name: "FK_Pin_User_UserId",
                table: "Pin");

            migrationBuilder.DropTable(
                name: "PinBoards");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Board",
                table: "Board");

            migrationBuilder.DropIndex(
                name: "IX_Board_UserId",
                table: "Board");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Board");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Board",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Board",
                table: "Board",
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
                        principalTable: "Board",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ImageSources_UserId",
                table: "ImageSources",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pin_Board_UserId",
                table: "Pin",
                column: "UserId",
                principalTable: "Board",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pin_ImageSources_BoardId",
                table: "Pin",
                column: "BoardId",
                principalTable: "ImageSources",
                principalColumn: "BoardId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
