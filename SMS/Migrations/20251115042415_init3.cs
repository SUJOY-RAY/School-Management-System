using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ListOfUsersId",
                table: "Classrooms",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Classrooms",
                keyColumn: "Id",
                keyValue: 1,
                column: "ListOfUsersId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Classrooms_ListOfUsersId",
                table: "Classrooms",
                column: "ListOfUsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_ListOfUsers_ListOfUsersId",
                table: "Classrooms",
                column: "ListOfUsersId",
                principalTable: "ListOfUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_ListOfUsers_ListOfUsersId",
                table: "Classrooms");

            migrationBuilder.DropIndex(
                name: "IX_Classrooms_ListOfUsersId",
                table: "Classrooms");

            migrationBuilder.DropColumn(
                name: "ListOfUsersId",
                table: "Classrooms");
        }
    }
}
