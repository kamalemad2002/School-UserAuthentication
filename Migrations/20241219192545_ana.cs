using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Migrations
{
    /// <inheritdoc />
    public partial class ana : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "userId",
                table: "kamal",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_kamal_userId",
                table: "kamal",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_kamal_kemo_userId",
                table: "kamal",
                column: "userId",
                principalTable: "kemo",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_kamal_kemo_userId",
                table: "kamal");

            migrationBuilder.DropIndex(
                name: "IX_kamal_userId",
                table: "kamal");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "kamal");
        }
    }
}
