namespace DotnetPageHeater.Models
{
    public class EventData
    {
        public string? Url { get; set; }

        public int Count { get; set; }
        public string? ElementPath { get; set; }
        public int ViewportWidth { get; set; }
        public int ViewportHeight { get; set; }
    }
}
