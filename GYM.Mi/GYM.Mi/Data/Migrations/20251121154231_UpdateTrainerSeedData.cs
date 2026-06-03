using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM.Mi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTrainerSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NextMaintenanceDate",
                table: "Equipments");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("056988a3-d645-4c7a-a453-bc0cae9f1748"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "Trainer", "TRAINER" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "NextMaintenanceDate",
                table: "Equipments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("056988a3-d645-4c7a-a453-bc0cae9f1748"),
                columns: new[] { "Name", "NormalizedName" },
                values: new object[] { "User", "USER" });
        }
    }
}
