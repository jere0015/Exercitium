using Exercitium.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Exercitium.ViewModels
{
    public class ExerciseStatisticsViewModel
    {
        public List<Exercise> Exercises { get; set; }
        public int? SelectedExerciseId { get; set; }
        public string DefaultExerciseName { get; set; }
        public List<DateTime> Dates { get; set; }
        public List<double> Weights { get; set; }
    }
}
