namespace Exercitium.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Type { get; set; } = string.Empty;
        public List<Exercise>? Exercises { get; set; }
    }
}
