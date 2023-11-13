using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Exercitium.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Exercitium.Data
{
    public class ExercitiumContext : IdentityDbContext<User>
    {
        public ExercitiumContext (DbContextOptions<ExercitiumContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           base.OnModelCreating(modelBuilder);
        }

        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Workout> Workouts { get; set; }
        public DbSet<WorkoutExercise> WorkoutExercises { get; set; }
        public DbSet<WorkoutProgram> WorkoutPrograms { get; set; }
        public DbSet<WorkoutProgramExercise> WorkoutProgramExercises { get; set; }
    }
}
