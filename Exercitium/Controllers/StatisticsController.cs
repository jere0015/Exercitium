using Exercitium.Models;
using Exercitium.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Exercitium.Data;
using Microsoft.AspNetCore.Authorization;

namespace Exercitium.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly ExercitiumContext _context;

        public StatisticsController(UserManager<User> userManager, ExercitiumContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public IActionResult Index()
        {
            var exercises = _context.Exercises.ToList();
            var viewModel = new ExerciseStatisticsViewModel
            {
                Exercises = exercises,
                SelectedExerciseId = exercises.FirstOrDefault()?.Id,
                DefaultExerciseName = exercises.FirstOrDefault()?.ExerciseName
            };
            return View(viewModel);
        }

        // GET: Statistics/GetExerciseData?exerciseId={exerciseId}
        public IActionResult GetExerciseData(int exerciseId)
        {
            var exercise = _context.Exercises.FirstOrDefault(e => e.Id == exerciseId);
            if (exercise == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);

            var workouts = _context.Workouts
                .Include(w => w.WorkoutExercises)
                .Where(w => w.UserId == userId)
                .ToList();

            var dates = new List<DateTime>();
            var weights = new List<double>();

            foreach (var workout in workouts)
            {
                var workoutExercise = workout.WorkoutExercises.FirstOrDefault(we => we.ExerciseId == exerciseId);
                if (workoutExercise != null)
                {
                    dates.Add(workout.DateTime);
                    weights.Add(workoutExercise.Weight);
                }
            }

            var sortedData = dates.Zip(weights, (d, w) => new { Date = d, Weight = w })
                          .OrderBy(d => d.Date)
                          .ToList();

            dates = sortedData.Select(d => d.Date).ToList();
            weights = sortedData.Select(d => d.Weight).ToList();

            var data = new { dates, weights };
            return Json(data);
        }

    }
}
