using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace House_renting_system_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixSeedCategoryIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CategoryId",
                value: 4);

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CategoryId",
                value: 4);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CategoryId",
                value: 6);

            migrationBuilder.UpdateData(
                table: "Houses",
                keyColumn: "Id",
                keyValue: 4,
                column: "CategoryId",
                value: 7);
        }
    }
}
