using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Migrations
{
    /// <inheritdoc />
    public partial class kamall : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_kamal_kemo_userId",
                table: "kamal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kemo",
                table: "kemo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_kamal",
                table: "kamal");

            migrationBuilder.RenameTable(
                name: "kemo",
                newName: "users");

            migrationBuilder.RenameTable(
                name: "kamal",
                newName: "students");

            migrationBuilder.RenameIndex(
                name: "IX_kamal_userId",
                table: "students",
                newName: "IX_students_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_users",
                table: "users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_students",
                table: "students",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_students_users_userId",
                table: "students",
                column: "userId",
                principalTable: "users",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_students_users_userId",
                table: "students");

            migrationBuilder.DropPrimaryKey(
                name: "PK_users",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_students",
                table: "students");

            migrationBuilder.RenameTable(
                name: "users",
                newName: "kemo");

            migrationBuilder.RenameTable(
                name: "students",
                newName: "kamal");

            migrationBuilder.RenameIndex(
                name: "IX_students_userId",
                table: "kamal",
                newName: "IX_kamal_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_kemo",
                table: "kemo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_kamal",
                table: "kamal",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_kamal_kemo_userId",
                table: "kamal",
                column: "userId",
                principalTable: "kemo",
                principalColumn: "Id");
        }
    }
}
