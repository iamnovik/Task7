using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace lab7.Migrations
{
    /// <inheritdoc />
    public partial class ContactsMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ContactRelatedId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ContactRelatedId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ContactRelatedId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "ContactRelationships",
                columns: table => new
                {
                    SourceContactId = table.Column<string>(type: "text", nullable: false),
                    TargetContactId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactRelationships", x => new { x.SourceContactId, x.TargetContactId });
                    table.ForeignKey(
                        name: "FK_ContactRelationships_AspNetUsers_SourceContactId",
                        column: x => x.SourceContactId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactRelationships_AspNetUsers_TargetContactId",
                        column: x => x.TargetContactId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SenderId = table.Column<string>(type: "text", nullable: false),
                    ReceiverId = table.Column<string>(type: "text", nullable: false),
                    Text = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactRelationships_TargetContactId",
                table: "ContactRelationships",
                column: "TargetContactId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactRelationships");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.AddColumn<string>(
                name: "ContactRelatedId",
                table: "AspNetUsers",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ContactRelatedId",
                table: "AspNetUsers",
                column: "ContactRelatedId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AspNetUsers_ContactRelatedId",
                table: "AspNetUsers",
                column: "ContactRelatedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
