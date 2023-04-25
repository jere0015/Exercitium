namespace Exercitium.ViewModels
{
    public class ExerciseViewModel
    {
        public int Id { get; set; }
        public string ExerciseName { get; set; } = string.Empty;
        public int Sets { get; set; }
        public int Reps { get; set; }
        public double Weight { get; set; }
        public bool IsSelected { get; set; }
    }
}
