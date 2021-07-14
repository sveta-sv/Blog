using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        /* Dependency Injection */
        private BlogDBContext db;
        private IConfiguration _config;
        public HomeController(BlogDBContext _db, IConfiguration config)
        {
            db = _db;
            _config = config;
        }

        /* Show searching fields and all posts */
        public async Task<IActionResult> Index(int? pageNumber, string titleOfPost, string categoryOfPost, DateTime? dateOfPost, string authorOfPost)
        {
            // Show chosen title, category, date and author of the post
            ViewData["FilterByTitle"] = titleOfPost;
            ViewData["FilterByCategory"] = categoryOfPost;
            ViewData["FilterByDate"] = dateOfPost?.ToString("yyyy-MM-dd");
            ViewData["FilterByAuthor"] = authorOfPost;

            // Working with database
            var database = new DBServices(db);
            var postsFromDb = await database.GetPosts(pageNumber, titleOfPost, categoryOfPost, dateOfPost, authorOfPost);
            var categoriesFromDb = (await database.GetCategories()).Select(x => new SelectListItem { Value = x.Id, Text = x.Name });

            // Collection of the posts with categories
            var posts = new ListPosts
            {
                Posts = postsFromDb,
                Categories = categoriesFromDb
            };

            // Go to the view Home/Index
            return View(posts);
        }

        /* Show the image of the post */
        public async Task<IActionResult> Image(string id)
        {
            // Working with database
            var database = new DBServices(db);
            var post = await database.GetPost(id);

            // The view Home/Index
            return File(post.ImageData, post.ImageType);
        }

        /* Show the full text of the post clicking the button Read more */
        public async Task<IActionResult> ReadPost(string id)
        {
            // Working with database
            var database = new DBServices(db);
            var post = await database.GetPost(id);
            var comments = await database.GetComments(id);

            // Passing to markdown extensions
            string pathToFolder = _config["VirtualPathToPostFiles"];
            string pathToSubFolder = post.Id;
            string fileName = "$1";
            string pathToFilesOfPost = Path.Combine(pathToFolder, pathToSubFolder, fileName);

            string postContent = post.Content;

            // Post and comments
            var currentPost = new ReadPost()
            {
                PostTitle = post.Name,
                //PostContent = post.Content,
                //PostContent = postContent,
                PostContent = MarkdownExtensions.ShowContentByMarkdown(pathToFilesOfPost, postContent),
                Comments = comments,
                PostId = post.Id
            };

            // Go to the view Home/ReadPost
            return View(currentPost);
        }

        /* Add comment to the post */
        public async Task<IActionResult> SaveComment(ReadPost userComment)
        {
            // Working with database
            var database = new DBServices(db);

            // Comment
            if (ModelState.IsValid)
            {
                var comment = new Comment();
                comment.Id = Guid.NewGuid().ToString();
                comment.UserId = User.Identity.GetUserId();
                comment.Text = userComment.Comment.Text;
                comment.Date = DateTime.UtcNow;
                comment.PostId = userComment.Comment.PostId;

                await database.AddComment(comment);
            }

            // Go to the view Home/ReadPost
            return RedirectToAction("ReadPost", "Home", new { id = userComment.Comment.PostId });
        }

        /* Delete comment of the post */
        public async Task<IActionResult> DeleteComment(string commentId)
        {
            // Working with database
            var database = new DBServices(db);
            var comment = await database.GetComment(commentId); // Obtain comment from db because we need PostId that will be passed to the action ReadPost
            if (comment.UserId != User.Identity.GetUserId()) return Unauthorized(); // Check if user authorized to delete the comment
            await database.DeleteComment(comment);

            //Go to the view Home/ReadPost
            return RedirectToAction("ReadPost", "Home", new { id = comment.PostId });
        }
    }
}
