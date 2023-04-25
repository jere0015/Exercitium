using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Exercitium.ViewModels
{
    public class WorkoutEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Type is required.")]
        public string Type { get; set; } = string.Empty;

        public ICollection<ExerciseViewModel>? Exercises { get; set; }
        public List<int> SelectedExerciseIds { get; set; }
        public List<SelectListItem> AvailableExercises { get; set; }
    }
}
