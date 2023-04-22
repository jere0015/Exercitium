using Exercitium.Data;
using Exercitium.Models;
using Exercitium.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Exercitium.Controllers
{
    [Authorize]
    public class WorkoutController : Controller
    {
        private readonly ExercitiumContext _context;
        private readonly UserManager<User> _userManager;

        public WorkoutController(ExercitiumContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
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
            var viewModel = new WorkoutCreateViewModel
            {
                Exercises = exercises.Select(e => new ExerciseViewModel
                {
                    Id = e.Id,
                    ExerciseName = e.ExerciseName
                }).ToList(),
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
                    UserId = userId.Id
                };

                foreach (var exercise in viewModel.Exercises)
                {
                    var exercises = await _context.Exercises.FindAsync(exercise.Id);
                    if (exercise != null)
                    {
                        var workoutExercise = new WorkoutExercise
                        {
                            Workout = workout,
                            Exercise = exercises,
                            Sets = exercise.Sets,
                            Reps = exercise.Reps,
                            Weight = exercise.Weight
                        };
                        workout.WorkoutExercises.Add(workoutExercise);
                    }
                }
                _context.Workouts.Add(workout);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Workout");
            }
            viewModel.Exercises = _context.Exercises.Select(e => new ExerciseViewModel
            {
                Id = e.Id,
                ExerciseName = e.ExerciseName
            }).ToList();
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
            return RedirectToAction("Index");
        }
    }
}
