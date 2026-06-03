using System.Diagnostics;
using GYM.Domain.Services;
using GYM.Mi.Models;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Mi.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogService _blogService;
        private readonly IEmployeeService _employeeService;


        public HomeController(
            ILogger<HomeController> logger,
            IBlogService blogService,
            IEmployeeService employeeService)
        {
            _logger = logger;
            _blogService = blogService;
            _employeeService = employeeService;
        }

        public IActionResult Index()
        {
            var latestBlogs = _blogService.GetLatestPublishedBlogs(3);
            var featuredTrainers = _employeeService.GetPublicLandingTrainers(3);

            var model = new HomeIndexModel
            {
                Blogs = latestBlogs.Select(blog => new LandingBlogModel
                {
                    Title = blog.Title,
                    Slug = blog.Slug,
                    ShortDescription = blog.ShortDescription,
                    FeaturedImageUrl = blog.FeaturedImageUrl,
                    AuthorName = blog.AuthorName,
                    PublishedAt = blog.PublishedAt
                }).ToList(),

                Trainers = featuredTrainers.Select(employee => new LandingTrainerModel
                {
                    FullName = $"{employee.FirstName} {employee.LastName}".Trim(),
                    ImageUrl = employee.ImageUrl,
                    Specialization = employee.Specialization,
                    ExperienceYears = employee.ExperienceYears,
                    ShortBio = employee.ShortBio
                }).ToList()
            };

            return View(model);
        }
        public IActionResult Trainers()
        {
            var trainers = _employeeService.GetPublicTrainers();

            var model = trainers.Select(employee => new LandingTrainerModel
            {
                FullName = $"{employee.FirstName} {employee.LastName}".Trim(),
                ImageUrl = employee.ImageUrl,
                Specialization = employee.Specialization,
                ExperienceYears = employee.ExperienceYears,
                ShortBio = employee.ShortBio
            }).ToList();

            return View(model);
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
