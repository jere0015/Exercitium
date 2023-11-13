using Exercitium.Data;
using Exercitium.Models;
using Exercitium.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercitium.Controllers
{
    public class WorkoutProgramController : Controller
    {
        private readonly ExercitiumContext _context;
        private readonly UserManager<User> _userManager;

        public WorkoutProgramController(ExercitiumContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Create()
        {
            var exercises = _context.Exercises.ToList();

            return View(exercises);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkoutProgramCreateViewModel viewModel)
        {
            User? user = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {
                var workoutProgram = new WorkoutProgram
                {
                    ProgramName = viewModel.ProgramName,
                    WorkoutType = viewModel.WorkoutType,
                    UserId = user.Id,
                    WorkoutProgramExercises = new List<WorkoutProgramExercise>()
                };
            }

            return View();
        }
    }
}
