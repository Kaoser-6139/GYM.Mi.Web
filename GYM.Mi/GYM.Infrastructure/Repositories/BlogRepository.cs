using GYM.Domain.Entities;
using GYM.Domain.Repositories;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GYM.Infrastructure.Repositories
{
    public class BlogRepository : Repository<Blog, Guid>, IBlogRepository
    {
        public BlogRepository(ApplicationDbContext context) : base(context)
        {
        }

        public (IList<Blog> data, int total, int totalDisplay) GetPagedBlogs(
            int pageIndex,
            int pageSize,
            string? order,
            DataTablesSearch search)
        {
            if (string.IsNullOrWhiteSpace(search.Value))
            {
                return GetDynamic(null, order, null, pageIndex, pageSize, true);
            }

            return GetDynamic(
                x => x.Title.Contains(search.Value) ||
                     x.Slug.Contains(search.Value) ||
                     x.AuthorName.Contains(search.Value) ||
                     x.ShortDescription.Contains(search.Value),
                order,
                null,
                pageIndex,
                pageSize,
                true);
        }

        public Blog? GetBySlug(string slug)
        {
            return GetDynamic(
                x => x.Slug == slug,
                orderBy: null,
                include: null,
                pageIndex: 1,
                pageSize: 1,
                isTrackingOff: true
            ).data.FirstOrDefault();
        }

        public Blog? GetPublishedBySlug(string slug)
        {
            return GetDynamic(
                x => x.Slug == slug && x.IsPublished,
                orderBy: null,
                include: null,
                pageIndex: 1,
                pageSize: 1,
                isTrackingOff: true
            ).data.FirstOrDefault();
        }

        public IList<Blog> GetPublishedBlogs()
        {
            return GetDynamic(
                x => x.IsPublished,
                orderBy: "PublishedAt desc",
                include: null,
                pageIndex: 1,
                pageSize: int.MaxValue,
                isTrackingOff: true
            ).data.ToList();
        }

        public IList<Blog> GetLatestPublishedBlogs(int count)
        {
            return GetDynamic(
                x => x.IsPublished,
                orderBy: "PublishedAt desc",
                include: null,
                pageIndex: 1,
                pageSize: count,
                isTrackingOff: true
            ).data.ToList();
        }
    }
}
