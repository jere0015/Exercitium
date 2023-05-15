using Exercitium.Models;
using System.ComponentModel.DataAnnotations;

namespace Exercitium.ViewModels
{
    public class WorkoutViewModelTest
    {
        [Required(ErrorMessage = "Please enter a date for the workout")]
        [Display(Name = "Workout Date")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Please select a workout type")]
        [Display(Name = "Workout Type")]
        public string Type { get; set; }

        public List<ExerciseViewModelTest> Exercises { get; set; }
        public Dictionary<int, ExerciseViewModelTest> ExerciseInputs { get; set; }
    }
}
