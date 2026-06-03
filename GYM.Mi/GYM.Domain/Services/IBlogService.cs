using GYM.Domain.Entities;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Services
{
    public interface IBlogService
    {
        void AddBlog(Blog blog);

        void DeleteBlog(Guid id);

        Blog? GetBlog(Guid id);

        Blog? GetBySlug(string slug);

        Blog? GetPublishedBySlug(string slug);

        IList<Blog> GetPublishedBlogs();

        IList<Blog> GetLatestPublishedBlogs(int count);

        (IList<Blog> data, int total, int totalDisplay) GetBlogs(
            int pageIndex,
            int pageSize,
            string? order,
            DataTablesSearch search);

        void Update(Blog blog);
    }
}
