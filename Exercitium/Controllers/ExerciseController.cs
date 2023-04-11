using Exercitium.Data;
using Exercitium.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Exercitium.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ExercitiumContext _dbContext;

        public ExerciseController(ExercitiumContext exercitiumContext)
        {
            _dbContext = exercitiumContext;
        }
        public ActionResult Index()
        {
            var exercises = _dbContext.Exercise.ToList();
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
                _dbContext.Exercise.Add(exercise);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(exercise);
        }

        public ActionResult Edit(int id)
        {
            var exercise = _dbContext.Exercise.Find(id);
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
                return RedirectToAction("Index");
            }
            return View(exercise);
        }

        public ActionResult Delete(int id)
        {
            var exercise = _dbContext.Exercise.Find(id);
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
            var exerciseToDelete = _dbContext.Exercise.Find(id);
            if (exerciseToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Exercise.Remove(exerciseToDelete);
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
