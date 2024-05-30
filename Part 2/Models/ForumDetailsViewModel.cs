namespace Part_2.Models
{
    public class ForumDetailsViewModel
    {
        public Forum Forum { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }

    public class PostDetailsViewModel
    {
        public Post Post { get; set; }
        public IEnumerable<Reply> Replies { get; set; }
    }


}
