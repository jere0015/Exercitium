namespace Exercitium.Models
{
    public class WorkoutProgram
    {
        public int Id { get; set; }
        public string ProgramName { get; set; }
        public string WorkoutType { get; set; }

        public int WorkoutId { get; set; }
        public Workout Workout { get; set; }

        public string? UserId { get; set; }
        public User User { get; set; }

        public ICollection<WorkoutProgramExercise>? WorkoutProgramExercises { get; set; }
    }
}
