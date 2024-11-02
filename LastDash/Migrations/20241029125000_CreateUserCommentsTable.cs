using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LastDash.Migrations
{
    /// <inheritdoc />
    public partial class CreateUserCommentsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserComments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(nullable: true), // Set to true if content can be empty
                    WrittenDate = table.Column<DateTime>(nullable: false),
                    PostId = table.Column<int>(nullable: false) // Add PostId column
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserComments_Posts_PostId", // Foreign key constraint
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade); // Set cascade delete behavior
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserComments_PostId", // Index on PostId for performance
                table: "UserComments",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserComments");
        }
    }
}
