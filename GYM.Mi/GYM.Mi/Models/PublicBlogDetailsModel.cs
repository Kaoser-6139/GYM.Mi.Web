namespace GYM.Mi.Models
{
    public class PublicBlogDetailsModel
    {
        public string Title { get; set; } = string.Empty;

        public string Slug { get; set; } = string.Empty;

        public string ShortDescription { get; set; } = string.Empty;

        public string FullContent { get; set; } = string.Empty;

        public string? FeaturedImageUrl { get; set; }

        public string AuthorName { get; set; } = string.Empty;

        public DateTime? PublishedAt { get; set; }
    }
}
