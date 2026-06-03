using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM.Mi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { new Guid("7d6739d9-44fa-4236-abb6-b2d855e3657c"), "4/19/2025 1:02:05 AM", "User", "USER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("7d6739d9-44fa-4236-abb6-b2d855e3657c"));
        }
    }
}
