using GYM.Domain.Entities;
using GYM.Domain.Services;
using GYM.Domain;
using GYM.Mi.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GYM.Application.Services
{
    public class BlogService : IBlogService
    {
        private readonly IApplicationUnitOfWork _applicationUnitOfWork;

        public BlogService(IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork = applicationUnitOfWork;
        }

        public void AddBlog(Blog blog)
        {
            blog.Id = Guid.NewGuid();
            blog.CreatedAt = DateTime.Now;
            blog.UpdatedAt = null;

            if (string.IsNullOrWhiteSpace(blog.Slug))
            {
                blog.Slug = GenerateSlug(blog.Title);
            }
            else
            {
                blog.Slug = GenerateSlug(blog.Slug);
            }

            blog.Slug = EnsureUniqueSlug(blog.Slug);

            if (blog.IsPublished)
            {
                blog.PublishedAt = DateTime.Now;
            }
            else
            {
                blog.PublishedAt = null;
            }

            _applicationUnitOfWork.BlogRepository.Add(blog);
            _applicationUnitOfWork.Save();
        }

        public void DeleteBlog(Guid id)
        {
            _applicationUnitOfWork.BlogRepository.Remove(id);
            _applicationUnitOfWork.Save();
        }

        public Blog? GetBlog(Guid id)
        {
            return _applicationUnitOfWork.BlogRepository.GetById(id);
        }

        public Blog? GetBySlug(string slug)
        {
            return _applicationUnitOfWork.BlogRepository.GetBySlug(slug);
        }

        public Blog? GetPublishedBySlug(string slug)
        {
            return _applicationUnitOfWork.BlogRepository.GetPublishedBySlug(slug);
        }

        public IList<Blog> GetPublishedBlogs()
        {
            return _applicationUnitOfWork.BlogRepository.GetPublishedBlogs();
        }

        public IList<Blog> GetLatestPublishedBlogs(int count)
        {
            return _applicationUnitOfWork.BlogRepository.GetLatestPublishedBlogs(count);
        }

        public (IList<Blog> data, int total, int totalDisplay) GetBlogs(
            int pageIndex,
            int pageSize,
            string? order,
            DataTablesSearch search)
        {
            return _applicationUnitOfWork.BlogRepository.GetPagedBlogs(
                pageIndex,
                pageSize,
                order,
                search);
        }

        public void Update(Blog blog)
        {
            var existingBlog = _applicationUnitOfWork.BlogRepository.GetById(blog.Id);

            if (existingBlog == null)
                return;

            var wasPublished = existingBlog.IsPublished;

            existingBlog.Title = blog.Title;
            existingBlog.ShortDescription = blog.ShortDescription;
            existingBlog.FullContent = blog.FullContent;
            existingBlog.FeaturedImageUrl = blog.FeaturedImageUrl;
            existingBlog.AuthorName = blog.AuthorName;
            existingBlog.IsPublished = blog.IsPublished;
            existingBlog.UpdatedAt = DateTime.Now;

            if (string.IsNullOrWhiteSpace(blog.Slug))
            {
                existingBlog.Slug = GenerateSlug(blog.Title);
            }
            else
            {
                existingBlog.Slug = GenerateSlug(blog.Slug);
            }

            existingBlog.Slug = EnsureUniqueSlug(existingBlog.Slug, existingBlog.Id);

            if (!wasPublished && existingBlog.IsPublished)
            {
                existingBlog.PublishedAt = DateTime.Now;
            }

            if (!existingBlog.IsPublished)
            {
                existingBlog.PublishedAt = null;
            }

            _applicationUnitOfWork.BlogRepository.Update(existingBlog);
            _applicationUnitOfWork.Save();
        }

        private string GenerateSlug(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return "blog-post";

            value = value.ToLower().Trim();

            value = Regex.Replace(value, @"[^a-z0-9\s-]", "");
            value = Regex.Replace(value, @"\s+", "-");
            value = Regex.Replace(value, @"-+", "-");

            return value.Trim('-');
        }

        private string EnsureUniqueSlug(string slug, Guid? currentBlogId = null)
        {
            var originalSlug = slug;
            var counter = 1;

            while (true)
            {
                var existingBlog = _applicationUnitOfWork.BlogRepository.GetBySlug(slug);

                if (existingBlog == null)
                    return slug;

                if (currentBlogId.HasValue && existingBlog.Id == currentBlogId.Value)
                    return slug;

                slug = $"{originalSlug}-{counter}";
                counter++;
            }
        }
    }
}
