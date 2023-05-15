using System.ComponentModel.DataAnnotations;

namespace Exercitium.ViewModels
{
    public class WorkoutCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Type is required")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
        public List<ExerciseViewModel> Exercises { get; set; }
        public List<SelectedExerciseViewModel> SelectedExercises { get; set; }
    }
}
