using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBorads.Migrations
{
    /// <inheritdoc />
    public partial class ViewTopAuthorsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Tags");

            migrationBuilder.Sql(@"
CREATE VIEW View_TopAuthors AS 
SELECT TOP 5 u.FullName, COUNT(*) as [WorkItemsCreated]
FROM Users u JOIN WorkItems wi on wi.AuthorId = u.Id
GROUP By u.Id , u.FullName
ORDER BY [WorkItemsCreated] desc");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 1,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 2,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 3,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 4,
                column: "Category",
                value: null);

            migrationBuilder.UpdateData(
                table: "Tags",
                keyColumn: "Id",
                keyValue: 5,
                column: "Category",
                value: null);

            migrationBuilder.Sql(@"
DROP VIEW View_TopAuthors ");

        }
    }
}
