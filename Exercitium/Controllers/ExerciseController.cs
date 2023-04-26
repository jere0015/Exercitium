using AspNetCoreHero.ToastNotification.Abstractions;
using Exercitium.Data;
using Exercitium.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Exercitium.Controllers
{
    [Authorize(Roles = "admin")]
    public class ExerciseController : Controller
    {
        private readonly ExercitiumContext _dbContext;
        private readonly INotyfService _notyf;

        public ExerciseController(ExercitiumContext exercitiumContext, INotyfService notyf)
        {
            _dbContext = exercitiumContext;
            _notyf = notyf;
        }
        public ActionResult Index()
        {
            var exercises = _dbContext.Exercises.ToList();
            return View(exercises);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Exercise exercise)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Exercises.Add(exercise);
                _dbContext.SaveChanges();
                _notyf.Success("Exercise was created");
                return RedirectToAction("Index");
            }
            _notyf.Error("Failed for some reasons");
            return View(exercise);
        }

        public ActionResult Edit(int id)
        {
            var exercise = _dbContext.Exercises.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Exercise exercise)
        {
            if (id != exercise.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                _dbContext.Entry(exercise).State = EntityState.Modified;
                _dbContext.SaveChanges();
                _notyf.Success("Exercise has been updated");
                return RedirectToAction("Index");
            }
            _notyf.Error("Failed for some reasons");
            return View(exercise);
        }

        public ActionResult Delete(int id)
        {
            var exercise = _dbContext.Exercises.Find(id);
            if (exercise == null)
            {
                return NotFound();
            }
            return View(exercise);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Exercise exercise)
        {
            var exerciseToDelete = _dbContext.Exercises.Find(id);
            if (exerciseToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Exercises.Remove(exerciseToDelete);
            _dbContext.SaveChanges();
            _notyf.Success("Exercise has been deleted");
            return RedirectToAction("Index");
        }
    }
}
