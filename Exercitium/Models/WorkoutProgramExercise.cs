namespace Exercitium.Models
{
    public class WorkoutProgramExercise
    {
        public int Id { get; set; }
        public int WorkoutProgramId { get; set; }
        public WorkoutProgram? WorkoutProgram { get; set; }

        public int ExerciseId { get; set; }
        public Exercise? Exercise { get; set; }
    }
}
