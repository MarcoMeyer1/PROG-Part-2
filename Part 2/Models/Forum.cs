using Microsoft.Extensions.Hosting;

namespace Part_2.Models
{
    public class Forum
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<Post> Posts { get; set; }
    }

    public class Post
    {
        public int Id { get; set; }
        public int ForumId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Forum Forum { get; set; }
        public User User { get; set; }
        public ICollection<Reply> Replies { get; set; }
    }

    public class Reply
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Post Post { get; set; }
        public User User { get; set; }
    }
}
