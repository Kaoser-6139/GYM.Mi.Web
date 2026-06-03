using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYM.Mi.Data.Migrations
{
    /// <inheritdoc />
    public partial class TrainerStudentRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TrainerEmployeeId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_TrainerEmployeeId",
                table: "Users",
                column: "TrainerEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Employees_TrainerEmployeeId",
                table: "Users",
                column: "TrainerEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Employees_TrainerEmployeeId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TrainerEmployeeId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TrainerEmployeeId",
                table: "Users");
        }
    }
}
