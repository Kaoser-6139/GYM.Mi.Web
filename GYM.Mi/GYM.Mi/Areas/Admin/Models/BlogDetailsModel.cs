namespace GYM.Mi.Areas.Admin.Models
{
    public class BlogDetailsModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public string FullContent { get; set; } = string.Empty;

        public string? FeaturedImageUrl { get; set; }

        public string AuthorName { get; set; } = string.Empty;

        public bool IsPublished { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? PublishedAt { get; set; }
    }
}
