using LastDash.Models; // Ensure you have the necessary using directive

namespace LastDash.ViewModels // Adjust to your actual namespace
{
    public class PostDetailsViewModel
    {
        public Post Post { get; set; } // Ensure this property exists
        public List<Post> RelatedPosts { get; set; }
        public List<UserComments> Comments { get; set; }
    }
}
