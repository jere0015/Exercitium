using AspNetCoreHero.ToastNotification.Abstractions;
using Exercitium.Data;
using Exercitium.Models;
using Exercitium.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Exercitium.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly ExercitiumContext _context;
        private readonly UserManager<User> _userManager;
        private readonly INotyfService _notyf;

        public WorkoutController(ExercitiumContext context, UserManager<User> userManager, INotyfService notyf)
        {
            _context = context;
            _userManager = userManager;
            _notyf = notyf;
        }

        public async Task<IActionResult> Index()
        {
            User? userId = await _userManager.GetUserAsync(User);

            var workouts = await _context.Workouts
                .Where(w => w.UserId == userId.Id)
                .ToListAsync();

            return View(workouts);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Retrieve workout details including associated exercises
            User? userId = await _userManager.GetUserAsync(User);
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId.Id);

            if (workout == null)
            {
                return NotFound();
            }

            // Map to view model
            var viewModel = new WorkoutDetailsViewModel
            {
                Id = workout.Id,
                DateTime = workout.DateTime,
                Type = workout.Type,
                Exercises = workout.WorkoutExercises
                    .Select(we => new ExerciseViewModel
                    {
                        Id = we.Exercise.Id,
                        ExerciseName = we.Exercise.ExerciseName,
                        Sets = we.Sets,
                        Reps = we.Reps,
                        Weight = we.Weight
                    }).ToList()
            };

            return View(viewModel);
        }
        public IActionResult Create()
        {
            var exercises = _context.Exercises.ToList();

            var exerciseViewModels = exercises.Select(e => new ExerciseViewModel
            {
                Id = e.Id,
                ExerciseName = e.ExerciseName,
                Sets = 0,
                Reps = 0,
                Weight = 0,
                IsSelected = false
            }).ToList();

            var viewModel = new WorkoutCreateViewModel
            {
                Exercises = exerciseViewModels
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WorkoutCreateViewModel viewModel)
        {
            User? userId = await _userManager.GetUserAsync(User);

            if (ModelState.IsValid)
            {

                var workout = new Workout
                {
                    Type = viewModel.Type,
                    DateTime = viewModel.DateTime,
                    UserId = userId.Id,
                    WorkoutExercises = new List<WorkoutExercise>()
                };

                var selectedExercises = viewModel.SelectedExercises;
                workout.WorkoutExercises = new List<WorkoutExercise>();

                for (int i = 0; i < selectedExercises.Count; i++)
                {
                    var selectedExercise = selectedExercises[i];
                    var exercises = await _context.Exercises.FindAsync(selectedExercise.ExerciseId);
                    if (exercises != null)
                    {
                        var workoutExercise = new WorkoutExercise
                        {
                            Workout = workout,
                            Exercise = exercises,
                            Sets = selectedExercise.Sets,
                            Reps = selectedExercise.Reps,
                            Weight = selectedExercise.Weight
                        };
                        workout.WorkoutExercises.Add(workoutExercise);
                    }
                }
                _context.Workouts.Add(workout);
                await _context.SaveChangesAsync();
                _notyf.Success("Workout has been created");
                return RedirectToAction("Index", "Workout");
            }

            // Log ModelState errors for debugging
            foreach (var modelStateEntry in ModelState)
            {
                var propertyName = modelStateEntry.Key;
                var errors = modelStateEntry.Value.Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"ModelState Error for property '{propertyName}': {error.ErrorMessage}");
                }
            }

            viewModel.Exercises = _context.Exercises.Select(e => new ExerciseViewModel
            {
                Id = e.Id,
                ExerciseName = e.ExerciseName
            }).ToList();
            _notyf.Error("Failed for some reasons");
            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int id)
        {
            User? userId = await _userManager.GetUserAsync(User);
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId.Id);

            if (workout == null)
            {
                return NotFound();
            }

            var viewModel = new WorkoutEditViewModel
            {
                Id = workout.Id,
                DateTime = workout.DateTime,
                Type = workout.Type,
                Exercises = workout.WorkoutExercises
                .Select(we => new ExerciseViewModel
                {
                    Id = we.Exercise.Id,
                    ExerciseName = we.Exercise.ExerciseName,
                    Sets = we.Sets,
                    Reps = we.Reps,
                    Weight = we.Weight
                }).ToList()
            };

            viewModel.AvailableExercises = await _context.Exercises
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.ExerciseName
                }).ToListAsync();

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WorkoutEditViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve existing workout from database
                    User? userId = await _userManager.GetUserAsync(User);
                    var workout = await _context.Workouts
                        .Include(w => w.WorkoutExercises)
                        .ThenInclude(we => we.Exercise)
                        .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId.Id);

                    if (workout == null)
                    {
                        return NotFound();
                    }

                    // Update workout properties
                    workout.DateTime = viewModel.DateTime;
                    workout.Type = viewModel.Type;

                    // Update associated exercises
                    var updatedExerciseIds = viewModel.Exercises.Select(e => e.Id).ToList();
                    var existingExercises = workout.WorkoutExercises.ToList();

                    // Remove exercises that are not in the updated list
                    foreach (var existingExercise in existingExercises)
                    {
                        if (!updatedExerciseIds.Contains(existingExercise.ExerciseId))
                        {
                            _context.WorkoutExercises.Remove(existingExercise);
                        }
                    }

                    // Add new exercises from the updated list
                    foreach (var updatedExerciseId in updatedExerciseIds)
                    {
                        if (!existingExercises.Any(we => we.ExerciseId == updatedExerciseId))
                        {
                            var exercise = await _context.Exercises.FindAsync(updatedExerciseId);
                            if (exercise != null)
                            {
                                var workoutExercise = new WorkoutExercise
                                {
                                    Workout = workout,
                                    Exercise = exercise
                                };
                                _context.WorkoutExercises.Add(workoutExercise);
                            }
                        }
                    }

                    _context.Update(workout);
                    _notyf.Success("Workout is now updated");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    User? userId = await _userManager.GetUserAsync(User);
                    bool workoutExists = _context.Workouts.Any(w => w.Id == id && w.UserId == userId.Id);
                    if (!workoutExists)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }

            viewModel.AvailableExercises = _context.Exercises.Select(e => new SelectListItem
            {
                Value = e.Id.ToString(),
                Text = e.ExerciseName
            }).ToList();
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            User? userId = await _userManager.GetUserAsync(User);
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId.Id);

            if (workout == null)
            {
                return NotFound();
            }

            return View(workout);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Retrieve workout details including associated exercises
            User? userId = await _userManager.GetUserAsync(User);
            var workout = await _context.Workouts
                .Include(w => w.WorkoutExercises)
                .ThenInclude(we => we.Exercise)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId.Id);

            if (workout == null)
            {
                return NotFound();
            }

            // Remove associated exercises
            var workoutExercises = workout.WorkoutExercises.ToList();
            foreach (var workoutExercise in workoutExercises)
            {
                _context.WorkoutExercises.Remove(workoutExercise);
            }

            // Remove workout
            _context.Workouts.Remove(workout);
            await _context.SaveChangesAsync();
            _notyf.Success("Workout has been deleted");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CreateTest()
        {
            var exercises = _context.Exercises.ToList();

            var exerciseViewModels = exercises.Select(e => new ExerciseViewModelTest
            {
                Id = e.Id,
                ExerciseName = e.ExerciseName,
                IsChecked = false
            }).ToList();

            var viewModel = new WorkoutViewModelTest
            {
                Exercises = exerciseViewModels
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTest(WorkoutViewModelTest model)
        {
            User? userId = await _userManager.GetUserAsync(User);

            // Check model state
            if (ModelState.IsValid)
            {
                // Create new workout and set properties
                var workout = new Workout
                {
                    Type = model.Type,
                    DateTime = model.Date,
                    UserId = userId.Id,
                    WorkoutExercises = new List<WorkoutExercise>()
                };

                // Loop through all exercises and add to database with corresponding sets, reps, and weights
                foreach (var exercise in model.Exercises)
                {
                    if (exercise.IsChecked)
                    {
                        if (model != null)
                        {
                            var workoutExercise = new WorkoutExercise
                            {
                                WorkoutId = workout.Id,
                                ExerciseId = exercise.Id
                            };

                            _context.WorkoutExercises.Add(workoutExercise);
                            await _context.SaveChangesAsync();
                        }

                    }
                }

                // Redirect to index page
                return RedirectToAction("Index", "Workout");
            }

            // If model state is not valid, return view with model
            return View(model);
        }
    }
}