namespace LastDash.Models
{
    public class HomePageViewModel
    {
        public List<string> Categories { get; set; }
        public Dictionary<string, List<Post>> Posts { get; set; }
    }
}
