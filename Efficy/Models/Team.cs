namespace Efficy.Models
{
    public class Team
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public List<StepCounter> StepCounters { get; set; } = new List<StepCounter>();

    }
}
