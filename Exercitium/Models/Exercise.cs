namespace Exercitium.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public ICollection<WorkoutExercise>? WorkoutExercises { get; set; }
        public ICollection<WorkoutProgramExercise>? WorkoutProgramExercises { get; set; }
    }
}
