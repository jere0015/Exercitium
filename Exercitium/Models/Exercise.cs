namespace Exercitium.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public List<Workout>? Workouts { get; set; }
    }
}
