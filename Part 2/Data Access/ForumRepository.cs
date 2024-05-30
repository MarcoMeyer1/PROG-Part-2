using Dapper;
using Part_2.Data_Access;
using Part_2.Models;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Part_2.Data
{
    public class ForumRepository
    {
        private readonly DatabaseContext _context;

        public ForumRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Forum>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Forum>("SELECT * FROM Forums");
            }
        }

        public async Task<Forum> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var forum = await connection.QuerySingleOrDefaultAsync<Forum>("SELECT * FROM Forums WHERE Id = @Id", new { Id = id });
                if (forum != null)
                {
                    var posts = await connection.QueryAsync<Post>("SELECT * FROM Posts WHERE ForumId = @ForumId", new { ForumId = id });
                    foreach (var post in posts)
                    {
                        post.User = await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @UserId", new { UserId = post.UserId });
                    }
                    forum.Posts = posts.AsList();
                }
                return forum;
            }
        }
    }

    public class PostRepository
    {
        private readonly DatabaseContext _context;
        private readonly ReplyRepository _replyRepository;

        public PostRepository(DatabaseContext context, ReplyRepository replyRepository)
        {
            _context = context;
            _replyRepository = replyRepository;
        }

        public async Task<Post> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var post = await connection.QuerySingleOrDefaultAsync<Post>("SELECT * FROM Posts WHERE Id = @Id", new { Id = id });
                if (post != null)
                {
                    post.User = await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @UserId", new { UserId = post.UserId });
                    var replies = await connection.QueryAsync<Reply>("SELECT * FROM Replies WHERE PostId = @PostId", new { PostId = id });
                    foreach (var reply in replies)
                    {
                        reply.User = await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @UserId", new { UserId = reply.UserId });
                    }
                    post.Replies = replies.AsList();
                }
                return post;
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "DELETE FROM Replies WHERE PostId = @Id";
                await connection.ExecuteAsync(sql, new { Id = id });

                sql = "DELETE FROM Posts WHERE Id = @Id";
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Post>> GetByForumIdAsync(int forumId)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "SELECT * FROM Posts WHERE ForumId = @ForumId";
                var posts = await connection.QueryAsync<Post>(sql, new { ForumId = forumId });
                foreach (var post in posts)
                {
                    post.User = await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @UserId", new { UserId = post.UserId });
                }
                return posts;
            }
        }

        public async Task AddAsync(Post post)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "INSERT INTO Posts (ForumId, UserId, Title, Content, CreatedAt) VALUES (@ForumId, @UserId, @Title, @Content, @CreatedAt)";
                await connection.ExecuteAsync(sql, post);
            }
        }
    }



    public class ReplyRepository
    {
        private readonly DatabaseContext _context;

        public ReplyRepository(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<Reply> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "SELECT * FROM Replies WHERE Id = @Id";
                var reply = await connection.QuerySingleOrDefaultAsync<Reply>(sql, new { Id = id });
                if (reply != null)
                {
                    reply.User = await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @UserId", new { UserId = reply.UserId });
                }
                return reply;
            }
        }

        public async Task AddAsync(Reply reply)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "INSERT INTO Replies (PostId, UserId, Content, CreatedAt) VALUES (@PostId, @UserId, @Content, @CreatedAt)";
                await connection.ExecuteAsync(sql, reply);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "DELETE FROM Replies WHERE Id = @Id";
                await connection.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<IEnumerable<Reply>> GetByPostIdAsync(int postId)
        {
            using (var connection = _context.CreateConnection())
            {
                var sql = "SELECT * FROM Replies WHERE PostId = @PostId";
                var replies = await connection.QueryAsync<Reply>(sql, new { PostId = postId });
                foreach (var reply in replies)
                {
                    reply.User = await connection.QuerySingleOrDefaultAsync<User>("SELECT * FROM Users WHERE Id = @UserId", new { UserId = reply.UserId });
                }
                return replies;
            }
        }
    }


}
