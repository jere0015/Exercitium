using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Exercitium.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public ICollection<Workout>? Workouts { get; set; }
        public ICollection<WorkoutProgram>? WorkoutPrograms { get; set; }
    }
}
