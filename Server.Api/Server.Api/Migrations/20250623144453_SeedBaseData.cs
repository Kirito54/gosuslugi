using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Server.Api.Server.Api.Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedBaseData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "8fd9f63e-0001-4000-8000-000000000001", null, "Специалист", "СПЕЦИАЛИСТ" },
                    { "8fd9f63e-0002-4000-8000-000000000002", null, "Начальник отдела", "НАЧАЛЬНИК ОТДЕЛА" },
                    { "8fd9f63e-0003-4000-8000-000000000003", null, "Начальник управления", "НАЧАЛЬНИК УПРАВЛЕНИЯ" }
                });

            migrationBuilder.InsertData(
                table: "Departments",
                columns: new[] { "Id", "Description", "Name", "ParentDepartmentId" },
                values: new object[,]
                {
                    { 1, null, "IT", null },
                    { 2, null, "Юридический", null },
                    { 3, null, "Кадры", null }
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "CreatedAt", "Description", "ExecutionDeadlineDate", "ExecutionDeadlineDays", "ExecutionStagesJson", "Name", "ResponsibleDepartmentId", "Status", "UpdatedAt" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Сервис по умолчанию", null, null, null, "Базовая услуга", null, "В процессе", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "CanApprove", "CanDelete", "CanEdit", "CanView", "DepartmentId", "Name", "Role", "ServiceId" },
                values: new object[,]
                {
                    { 1, false, false, false, true, 1, "Доступ к заявлениям", "Специалист", 1 },
                    { 2, false, false, true, false, 2, "Редактирование договоров", "Начальник отдела", 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8fd9f63e-0001-4000-8000-000000000001");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8fd9f63e-0002-4000-8000-000000000002");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8fd9f63e-0003-4000-8000-000000000003");

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Departments",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
