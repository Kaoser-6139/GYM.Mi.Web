using AutoMapper;
using GYM.Domain.Entities;
using GYM.Domain.Services;
using GYM.Infrastructure;
using GYM.Mi.Areas.Admin.Models;
using GYM.Mi.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GYM.Mi.Areas.Admin.Controllers
{
    [Area("Admin"), Authorize(Roles = "Admin,Manager")]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ILogger<BlogsController> _logger;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogsController(
            IBlogService blogService,
            ILogger<BlogsController> logger,
            IMapper mapper,
            IWebHostEnvironment webHostEnvironment)
        {
            _blogService = blogService;
            _logger = logger;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var (data, total, totalDisplay) = _blogService.GetBlogs(
                pageIndex: 1,
                pageSize: int.MaxValue,
                order: "CreatedAt desc",
                search: new DataTablesSearch()
            );

            var model = data.Select(blog => new BlogListItemModel
            {
                Id = blog.Id,
                Title = blog.Title,
                AuthorName = blog.AuthorName,
                IsPublished = blog.IsPublished,
                CreatedAt = blog.CreatedAt,
                PublishedAt = blog.PublishedAt
            }).ToList();

            return View(model);
        }

        public IActionResult Add()
        {
            var model = new AddBlogModel();
            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Add(AddBlogModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var blog = _mapper.Map<Blog>(model);

                    blog.Id = IdentityGenerator.NewSequentialGuid();

                    var uploadedImagePath = SaveBlogImage(model.FeaturedImage);

                    if (!string.IsNullOrWhiteSpace(uploadedImagePath))
                    {
                        blog.FeaturedImageUrl = uploadedImagePath;
                    }
                    else
                    {
                        blog.FeaturedImageUrl = model.FeaturedImageUrl;
                    }

                    _blogService.AddBlog(blog);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Blog post added successfully.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to add Blog post");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = ex.Message,
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);
        }

        public IActionResult Update(Guid id)
        {
            var blog = _blogService.GetBlog(id);

            if (blog == null)
            {
                return NotFound();
            }

            var model = new UpdateBlogModel();

            _mapper.Map(blog, model);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Update(UpdateBlogModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var blog = _mapper.Map<Blog>(model);

                    var uploadedImagePath = SaveBlogImage(model.FeaturedImage);

                    if (!string.IsNullOrWhiteSpace(uploadedImagePath))
                    {
                        blog.FeaturedImageUrl = uploadedImagePath;
                    }
                    else
                    {
                        blog.FeaturedImageUrl = model.FeaturedImageUrl;
                    }

                    _blogService.Update(blog);

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = "Blog post updated successfully.",
                        Type = ResponseTypes.Success
                    });

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to update Blog post");

                    TempData.Put("ResponseMessage", new ResponseModel
                    {
                        Message = ex.Message,
                        Type = ResponseTypes.Danger
                    });
                }
            }

            return View(model);
        }

        public IActionResult Details(Guid id)
        {
            var blog = _blogService.GetBlog(id);

            if (blog == null)
            {
                return NotFound();
            }

            var model = new BlogDetailsModel();

            _mapper.Map(blog, model);

            return View(model);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Delete(Guid id)
        {
            try
            {
                _blogService.DeleteBlog(id);

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Blog post deleted successfully.",
                    Type = ResponseTypes.Success
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete Blog post");

                TempData.Put("ResponseMessage", new ResponseModel
                {
                    Message = "Failed to delete Blog post.",
                    Type = ResponseTypes.Danger
                });
            }

            return RedirectToAction("Index");
        }

        private string? SaveBlogImage(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return null;
            }

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                throw new Exception("Only JPG, JPEG, PNG, and WEBP image files are allowed.");
            }

            var maxFileSize = 2 * 1024 * 1024; // 2 MB

            if (imageFile.Length > maxFileSize)
            {
                throw new Exception("Image size cannot be more than 2 MB.");
            }

            var uploadsFolder = Path.Combine(
                _webHostEnvironment.WebRootPath,
                "uploads",
                "blogs"
            );

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }

            return $"/uploads/blogs/{uniqueFileName}";
        }
    }
}
