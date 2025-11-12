using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SMS.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Classroom_ClassroomID",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Classroom_Schools_SchoolID",
                table: "Classroom");

            migrationBuilder.DropForeignKey(
                name: "FK_ListOfUsers_Classroom_ClassroomID",
                table: "ListOfUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Classroom_ClassroomID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_ClassroomID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_ListOfUsers_ClassroomID",
                table: "ListOfUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classroom",
                table: "Classroom");

            migrationBuilder.DropColumn(
                name: "ClassroomID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ClassroomID",
                table: "ListOfUsers");

            migrationBuilder.RenameTable(
                name: "Classroom",
                newName: "Classrooms");

            migrationBuilder.RenameIndex(
                name: "IX_Classroom_SchoolID",
                table: "Classrooms",
                newName: "IX_Classrooms_SchoolID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classrooms",
                table: "Classrooms",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "ClassroomUsers",
                columns: table => new
                {
                    ClassroomsId = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassroomUsers", x => new { x.ClassroomsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ClassroomUsers_Classrooms_ClassroomsId",
                        column: x => x.ClassroomsId,
                        principalTable: "Classrooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClassroomUsers_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_ClassroomUsers_UsersId",
                table: "ClassroomUsers",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Classrooms_ClassroomID",
                table: "Attendance",
                column: "ClassroomID",
                principalTable: "Classrooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classrooms_Schools_SchoolID",
                table: "Classrooms",
                column: "SchoolID",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attendance_Classrooms_ClassroomID",
                table: "Attendance");

            migrationBuilder.DropForeignKey(
                name: "FK_Classrooms_Schools_SchoolID",
                table: "Classrooms");

            migrationBuilder.DropTable(
                name: "ClassroomUsers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Classrooms",
                table: "Classrooms");

            migrationBuilder.RenameTable(
                name: "Classrooms",
                newName: "Classroom");

            migrationBuilder.RenameIndex(
                name: "IX_Classrooms_SchoolID",
                table: "Classroom",
                newName: "IX_Classroom_SchoolID");

            migrationBuilder.AddColumn<int>(
                name: "ClassroomID",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ClassroomID",
                table: "ListOfUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Classroom",
                table: "Classroom",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "ListOfUsers",
                keyColumn: "Id",
                keyValue: 1,
                column: "ClassroomID",
                value: null);

            migrationBuilder.UpdateData(
                table: "ListOfUsers",
                keyColumn: "Id",
                keyValue: 2,
                column: "ClassroomID",
                value: null);

            migrationBuilder.UpdateData(
                table: "ListOfUsers",
                keyColumn: "Id",
                keyValue: 3,
                column: "ClassroomID",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ListOfUsers",
                keyColumn: "Id",
                keyValue: 4,
                column: "ClassroomID",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ClassroomID",
                table: "Users",
                column: "ClassroomID");

            migrationBuilder.CreateIndex(
                name: "IX_ListOfUsers_ClassroomID",
                table: "ListOfUsers",
                column: "ClassroomID");

            migrationBuilder.AddForeignKey(
                name: "FK_Attendance_Classroom_ClassroomID",
                table: "Attendance",
                column: "ClassroomID",
                principalTable: "Classroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Classroom_Schools_SchoolID",
                table: "Classroom",
                column: "SchoolID",
                principalTable: "Schools",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ListOfUsers_Classroom_ClassroomID",
                table: "ListOfUsers",
                column: "ClassroomID",
                principalTable: "Classroom",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Classroom_ClassroomID",
                table: "Users",
                column: "ClassroomID",
                principalTable: "Classroom",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
