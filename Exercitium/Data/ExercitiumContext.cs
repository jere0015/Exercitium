using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Exercitium.Models;

namespace Exercitium.Data
{
    public class ExercitiumContext : DbContext
    {
        public ExercitiumContext (DbContextOptions<ExercitiumContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }

        public DbSet<Exercise> Exercise { get; set; }
        public DbSet<Workout> Workout { get; set; }
        public DbSet<User> User { get; set; }
    }
}
