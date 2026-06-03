using System.ComponentModel.DataAnnotations;

namespace GYM.Mi.Areas.Admin.Models
{
    public class AddBlogModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(250)]
        public string? Slug { get; set; }

        [Required]
        [MaxLength(500)]
        public string ShortDescription { get; set; } = string.Empty;

        [Required]
        public string FullContent { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? FeaturedImageUrl { get; set; }
        public IFormFile? FeaturedImage { get; set; }

        [Required]
        [MaxLength(100)]
        public string AuthorName { get; set; } = "GYM.Mi Team";

        public bool IsPublished { get; set; }
    }
}
