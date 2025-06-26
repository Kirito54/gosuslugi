using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Server.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicationFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalNumber",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApplicantName",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RepresentativeName",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Applications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Source",
                table: "Applications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RegistrarId",
                table: "Applications",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Applications_RegistrarId",
                table: "Applications",
                column: "RegistrarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Applications_AspNetUsers_RegistrarId",
                table: "Applications",
                column: "RegistrarId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Applications_AspNetUsers_RegistrarId",
                table: "Applications");

            migrationBuilder.DropIndex(
                name: "IX_Applications_RegistrarId",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ExternalNumber",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "ApplicantName",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "RepresentativeName",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "Applications");

            migrationBuilder.DropColumn(
                name: "RegistrarId",
                table: "Applications");
        }
    }
}
