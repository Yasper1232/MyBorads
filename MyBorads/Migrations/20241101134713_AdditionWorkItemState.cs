using Microsoft.EntityFrameworkCore.Migrations;
using MyBorads.Entities;

#nullable disable

namespace MyBorads.Migrations
{
    /// <inheritdoc />
    public partial class AdditionWorkItemState : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "WorkItemsStates",
                column: "Value",
                value: "On Hold");

            migrationBuilder.InsertData(
                table: "WorkItemsStates",
          column: "Value",
          value: "Rejected");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "WorkItemsStates",
                keyColumn: "Value",
                keyValue: "On Hold");

            migrationBuilder.DeleteData(
               table: "WorkItemsStates",
               keyColumn: "Value",
               keyValue: "Rejected");


        }
    }
}
