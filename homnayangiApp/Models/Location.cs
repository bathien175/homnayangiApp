namespace homnayangiApp.Models
{
    public class Location
    {
        public string Id { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Address { get; set; } = String.Empty;
        public string Province { get; set; } = String.Empty;
        public string District { get; set; } = String.Empty;
        public long MinPrice { get; set; }
        public long MaxPrice { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool IsOpen24H { get; set; } = false;
        public string HotLine { get; set; } = String.Empty;
        public List<string>? Images { get; set; }
        public List<string> Tags { get; set; } = new List<string>();
        public string? Creator { get; set; }
        public bool IsShare { get; set; } = false;
    }
}
