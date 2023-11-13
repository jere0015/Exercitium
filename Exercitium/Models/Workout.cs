namespace Exercitium.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; } = string.Empty;
        public ICollection<WorkoutExercise> WorkoutExercises { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }

        public WorkoutProgram WorkoutProgram { get; set; }
    }
}
