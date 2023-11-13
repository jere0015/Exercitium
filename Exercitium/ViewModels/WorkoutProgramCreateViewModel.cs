namespace Exercitium.ViewModels
{
    public class WorkoutProgramCreateViewModel
    {
        public int Id { get; set; }
        public string ProgramName { get; set; }
        public string WorkoutType { get; set; }
        public List<ExerciseViewModel> Exercises { get; set; }
    }
}
