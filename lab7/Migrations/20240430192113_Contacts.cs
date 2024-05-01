using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace lab7.Migrations
{
    /// <inheritdoc />
    public partial class Contacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactRelations");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
                name: "ContactRelations",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContactId = table.Column<string>(type: "text", nullable: false),
                    RelatedContactId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactRelations", x => x.id);
                    table.ForeignKey(
                        name: "FK_ContactRelations_Contact_ContactId",
                        column: x => x.ContactId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactRelations_Contact_RelatedContactId",
                        column: x => x.RelatedContactId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactRelations_ContactId",
                table: "ContactRelations",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactRelations_RelatedContactId",
                table: "ContactRelations",
                column: "RelatedContactId");
        }
    }
}
