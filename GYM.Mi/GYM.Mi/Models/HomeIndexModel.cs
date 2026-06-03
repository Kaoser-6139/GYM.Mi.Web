namespace GYM.Mi.Models
{
    public class HomeIndexModel
    {
        public IList<LandingBlogModel> Blogs { get; set; } = new List<LandingBlogModel>();
        public IList<LandingTrainerModel> Trainers { get; set; } = new List<LandingTrainerModel>();
    }
}
