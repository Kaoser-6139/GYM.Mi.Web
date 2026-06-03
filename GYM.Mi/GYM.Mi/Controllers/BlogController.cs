using GYM.Domain.Services;
using GYM.Mi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Mi.Controllers
{
    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly IBlogService _blogService;

        public BlogController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            var blogs = _blogService.GetPublishedBlogs();

            var model = blogs.Select(blog => new LandingBlogModel
            {
                Title = blog.Title,
                Slug = blog.Slug,
                ShortDescription = blog.ShortDescription,
                FeaturedImageUrl = blog.FeaturedImageUrl,
                AuthorName = blog.AuthorName,
                PublishedAt = blog.PublishedAt
            }).ToList();

            return View(model);
        }

        [HttpGet("{slug}")]
        public IActionResult Details(string slug)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                return NotFound();
            }

            var blog = _blogService.GetPublishedBySlug(slug);

            if (blog == null)
            {
                return NotFound();
            }

            var model = new PublicBlogDetailsModel
            {
                Title = blog.Title,
                Slug = blog.Slug,
                ShortDescription = blog.ShortDescription,
                FullContent = blog.FullContent,
                FeaturedImageUrl = blog.FeaturedImageUrl,
                AuthorName = blog.AuthorName,
                PublishedAt = blog.PublishedAt
            };

            return View(model);
        }
    }
}
