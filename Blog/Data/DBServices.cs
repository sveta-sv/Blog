using Blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Blog.Data
{
    public class DBServices
    {
        /* Number of posts on the page */
        private const int pageSize = 2;

        /* Dependency Injection */
        private BlogDBContext db;
        public DBServices(BlogDBContext _db)
        {
            db = _db;
        }

        /* Get all posts from the database */
        public async Task<PaginatedList<Post>> GetPosts(int? pageNumber, string postTitle, string categoryId, DateTime? publicationDate, string author)
        {
            // All posts
            var posts = from item in db.Posts
                        .Include(x => x.User)
                        .Include(x => x.Category)
                        select item;

            // Only posts filtered by ...
            if (!string.IsNullOrEmpty(postTitle))
            {
                posts = posts.Where(x => x.Name.Contains(postTitle));
            }
            if (!string.IsNullOrEmpty(author))
            {
                posts = posts.Where(x => x.User.Nickname.Contains(author));
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                posts = posts.Where(x => x.CategoryId == categoryId);
            }
            if (publicationDate.HasValue)
            {
                posts = posts.Where(x => x.Date.Value.Date == publicationDate.Value.Date);
            }

            // Sorting posts by date in ascending order
            posts = posts.OrderBy(x => x.Date);

            // Passing to action Index of Home contoller
            return await PaginatedList<Post>.CreateAsync(posts, pageNumber ?? 1, pageSize);
        }

        /* Get all categories of the posts from the database */
        public async Task<IEnumerable<Category>> GetCategories()
        {
            // All categories
            var categories = await (from item in db.Categories
                                    select item)
                                    .ToListAsync();

            // Passing to action Index of Home contoller
            return categories;
        }

        /* Get post from the database by id */
        public async Task<Post> GetPost(string id)
        {
            // One post
            var post = (from item in db.Posts
                        .Include(x => x.Category)
                        where item.Id == id
                        select item)
                        .SingleOrDefault();

            // Passing to action Image of Home and ReadPost of Home controller
            return await Task.FromResult(post);
        }

        /* Get comments of the post from the database */
        public async Task<IEnumerable<Comment>> GetComments(string id)
        {
            // All comments of the post
            var comments = await (from item in db.Comments
                                  .Include(x => x.User)
                                  where item.PostId == id
                                  select item)
                                  .OrderByDescending(x => x.Date)
                                  .ToListAsync();

            // Passing to action ReadPost of Home controller
            return comments;
        }

        /* Save new comment to database */
        public async Task AddComment(Comment comment)
        {
            await db.AddAsync(comment);
            await db.SaveChangesAsync();
        }

        /* Get comment from the database by id */
        public async Task<Comment> GetComment(string id)
        {
            // One comment
            var comment = (from item in db.Comments
                           where item.Id == id
                           select item)
                           .SingleOrDefault();

            // Passing to action DeleteComment of Home controller
            return await Task.FromResult(comment);
        }

        /* Delete comment from the database */
        public async Task DeleteComment(Comment comment)
        {
            db.Comments.Remove(comment);
            await db.SaveChangesAsync();
        }

        /* Get user's post from dstabase */
        public async Task<PaginatedList<Post>> GetUserPosts(int? pageNumber, string categoryOfPost, string userId, string categoryId, DateTime? dateOfPost)
        {
            // All user's posts
            var posts = from item in db.Posts
                        .Include(x => x.Category)
                        where item.UserId == userId
                        select item;

            // Only posts filtered by ...
            if (!string.IsNullOrEmpty(categoryOfPost))
            {
                posts = posts.Where(x => x.Name.Contains(categoryOfPost));
            }
            if (!string.IsNullOrEmpty(categoryId))
            {
                posts = posts.Where(x => x.CategoryId == categoryId);
            }
            if (dateOfPost.HasValue)
            {
                posts = posts.Where(x => x.Date.Value.Date == dateOfPost.Value.Date);
            }

            // Sorting posts by date in ascending order
            posts = posts.OrderBy(x => x.Date);

            // Passing to action Index of PersonalArea contoller
            return await PaginatedList<Post>.CreateAsync(posts, pageNumber ?? 1, pageSize);
        }

        /* Delete post from the database */
        public async Task DeletePost(Post post)
        {
            db.Posts.Remove(post);

            await db.SaveChangesAsync();
        }

        /* Modify post */
        public async Task EditPost(Post modifiedPost)
        {
            // Get post for modification
            var post = (from item in db.Posts
                        where item.Id == modifiedPost.Id
                        select item)
                        .SingleOrDefault();
            
            // Modifications
            post.Name = modifiedPost.Name;
            post.Content = modifiedPost.Content;
            post.Date = DateTime.UtcNow;
            post.ImageData = modifiedPost.ImageData;
            post.ImageName = modifiedPost.ImageName;
            post.ImageType = modifiedPost.ImageType;
            post.CategoryId = modifiedPost.CategoryId;
            
            // Save
            await db.SaveChangesAsync();
        }

        /* Create new post */
        public async Task AddNewPost(Post post)
        {
            await db.AddAsync(post);
            await db.SaveChangesAsync();
        }

        /* Save new category to database */
        public async Task AddCategory(Category category)
        {
            await db.AddAsync(category);
            await db.SaveChangesAsync();
        }
    }
}
