namespace homnayangiApp.Models
{
    public class User
    {
        public string Id { get; set; } = String.Empty;
        public string IDUser { get; set; } = String.Empty;
        public string Phone { get; set; } = String.Empty;
        public string Name { get; set; } = String.Empty;
        public string Gender { get; set; } = String.Empty;
        public string DateBirth { get; set; } = DateTime.Now.Date.ToString("yyyy-MM-dd");
        public string City { get; set; } = String.Empty;
        public String Dictrict { get; set; } = String.Empty;
        public string? ImageData { get; set; }
        public List<string>? SaveStore { get; set; }
        public string Password { get; set; } = String.Empty;
        public List<string> Tags { get; set; } = new List<string>();

    }
}
