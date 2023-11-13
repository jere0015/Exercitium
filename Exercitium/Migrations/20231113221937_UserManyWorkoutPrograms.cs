using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exercitium.Migrations
{
    /// <inheritdoc />
    public partial class UserManyWorkoutPrograms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "WorkoutPrograms",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPrograms_UserId",
                table: "WorkoutPrograms",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutPrograms_AspNetUsers_UserId",
                table: "WorkoutPrograms",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutPrograms_AspNetUsers_UserId",
                table: "WorkoutPrograms");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutPrograms_UserId",
                table: "WorkoutPrograms");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "WorkoutPrograms");
        }
    }
}
