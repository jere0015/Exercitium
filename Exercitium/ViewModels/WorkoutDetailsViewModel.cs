using Exercitium.Models;

namespace Exercitium.ViewModels
{
    public class WorkoutDetailsViewModel
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; } = string.Empty;
        public ICollection<ExerciseViewModel>? Exercises { get; set; }
        public ICollection<WorkoutExercise>? WorkoutExercises { get; set; }
    }
}
