using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig_rel_with_blograyting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BlogRaytings_BlogID",
                table: "BlogRaytings",
                column: "BlogID",
                unique: true,
                filter: "[BlogID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogRaytings_Blogs_BlogID",
                table: "BlogRaytings",
                column: "BlogID",
                principalTable: "Blogs",
                principalColumn: "BlogID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogRaytings_Blogs_BlogID",
                table: "BlogRaytings");

            migrationBuilder.DropIndex(
                name: "IX_BlogRaytings_BlogID",
                table: "BlogRaytings");
        }
    }
}
