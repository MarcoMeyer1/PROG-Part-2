using Microsoft.AspNetCore.Mvc;
using Part_2.Data;
using Part_2.Models;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Part_2.Controllers
{
    public class ForumsController : Controller
    {
        private readonly ForumRepository _forumRepository;
        private readonly PostRepository _postRepository;
        private readonly ReplyRepository _replyRepository;
        private readonly UserRepository _userRepository;

        public ForumsController(ForumRepository forumRepository, PostRepository postRepository, ReplyRepository replyRepository, UserRepository userRepository)
        {
            _forumRepository = forumRepository;
            _postRepository = postRepository;
            _replyRepository = replyRepository;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            var forums = await _forumRepository.GetAllAsync();
            return View(forums);
        }

        public async Task<IActionResult> ForumDetails(int id)
        {
            var forum = await _forumRepository.GetByIdAsync(id);
            if (forum == null)
            {
                return NotFound();
            }
            var posts = await _postRepository.GetByForumIdAsync(id);
            var viewModel = new ForumDetailsViewModel
            {
                Forum = forum,
                Posts = posts
            };
            return View(viewModel);
        }

        public async Task<IActionResult> PostDetails(int id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            var replies = await _replyRepository.GetByPostIdAsync(id);
            var viewModel = new PostDetailsViewModel
            {
                Post = post,
                Replies = replies
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(int forumId, string title, string content)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _userRepository.GetUserByEmailAsync(userEmail);
                if (user != null)
                {
                    var post = new Post
                    {
                        ForumId = forumId,
                        Title = title,
                        Content = content,
                        UserId = user.Id,
                        CreatedAt = DateTime.UtcNow // Ensure CreatedAt is set to current UTC time
                    };
                    await _postRepository.AddAsync(post);
                    return RedirectToAction("ForumDetails", new { id = forumId });
                }
            }
            return RedirectToAction("ForumDetails", new { id = forumId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateReply(int postId, string content)
        {
            if (ModelState.IsValid)
            {
                var userEmail = User.FindFirstValue(ClaimTypes.Email);
                var user = await _userRepository.GetUserByEmailAsync(userEmail);
                if (user != null)
                {
                    var reply = new Reply
                    {
                        PostId = postId,
                        Content = content,
                        UserId = user.Id,
                        CreatedAt = DateTime.UtcNow // Ensure CreatedAt is set to current UTC time
                    };
                    await _replyRepository.AddAsync(reply);
                    return RedirectToAction("PostDetails", new { id = postId });
                }
            }
            return RedirectToAction("PostDetails", new { id = postId });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePost(int postId)
        {
            if (User.IsInRole("Employee"))
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post != null)
                {
                    await _postRepository.DeleteAsync(postId);
                    return RedirectToAction("ForumDetails", new { id = post.ForumId });
                }
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReply(int replyId)
        {
            if (User.IsInRole("Employee"))
            {
                var reply = await _replyRepository.GetByIdAsync(replyId);
                if (reply != null)
                {
                    await _replyRepository.DeleteAsync(replyId);
                    return RedirectToAction("PostDetails", new { id = reply.PostId });
                }
            }
            return RedirectToAction("Index");
        }
    }
}
