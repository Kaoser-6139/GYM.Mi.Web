using GYM.Domain.Entities;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Domain.Repositories
{
    public interface IBlogRepository : IRepository<Blog, Guid>
    {
        (IList<Blog> data, int total, int totalDisplay) GetPagedBlogs(
            int pageIndex,
            int pageSize,
            string? order,
            DataTablesSearch search);

        Blog? GetBySlug(string slug);

        Blog? GetPublishedBySlug(string slug);

        IList<Blog> GetPublishedBlogs();

        IList<Blog> GetLatestPublishedBlogs(int count);
    }
}
