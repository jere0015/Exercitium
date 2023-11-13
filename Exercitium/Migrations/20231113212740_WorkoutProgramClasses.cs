using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Exercitium.Migrations
{
    /// <inheritdoc />
    public partial class WorkoutProgramClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkoutPrograms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProgramName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkoutType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorkoutId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutPrograms_Workouts_WorkoutId",
                        column: x => x.WorkoutId,
                        principalTable: "Workouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutProgramExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutProgramId = table.Column<int>(type: "int", nullable: false),
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutProgramExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutProgramExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutProgramExercises_WorkoutPrograms_WorkoutProgramId",
                        column: x => x.WorkoutProgramId,
                        principalTable: "WorkoutPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutProgramExercises_ExerciseId",
                table: "WorkoutProgramExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutProgramExercises_WorkoutProgramId",
                table: "WorkoutProgramExercises",
                column: "WorkoutProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPrograms_WorkoutId",
                table: "WorkoutPrograms",
                column: "WorkoutId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkoutProgramExercises");

            migrationBuilder.DropTable(
                name: "WorkoutPrograms");
        }
    }
}
