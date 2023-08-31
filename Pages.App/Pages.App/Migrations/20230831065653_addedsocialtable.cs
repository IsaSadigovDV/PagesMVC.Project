using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pages.App.Migrations
{
    public partial class addedsocialtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Socials_Authors_AuthorId",
                table: "Socials");

            migrationBuilder.DropIndex(
                name: "IX_Socials_AuthorId",
                table: "Socials");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Socials");

            migrationBuilder.CreateTable(
                name: "AuthorSocials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AuthorId = table.Column<int>(type: "int", nullable: false),
                    SocialId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorSocials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorSocials_Authors_AuthorId",
                        column: x => x.AuthorId,
                        principalTable: "Authors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuthorSocials_Socials_SocialId",
                        column: x => x.SocialId,
                        principalTable: "Socials",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSocials_AuthorId",
                table: "AuthorSocials",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_AuthorSocials_SocialId",
                table: "AuthorSocials",
                column: "SocialId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorSocials");

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Socials",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Socials_AuthorId",
                table: "Socials",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Socials_Authors_AuthorId",
                table: "Socials",
                column: "AuthorId",
                principalTable: "Authors",
                principalColumn: "Id");
        }
    }
}
