using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Authorize]
    public class PersonalAreaController : Controller
    {
        /* Dependency Injection */
        private BlogDBContext db;
        public PersonalAreaController(BlogDBContext _db)
        {
            db = _db;
        }

        /* Show searching fields and all posts of the logged user */
        public async Task<IActionResult> Index(int? pageNumber, string titleOfPost, string categoryOfPost, DateTime? dateOfPost)
        {
            // Show chosen title, category, date of the post
            ViewData["FilterByTitle"] = titleOfPost;
            ViewData["FilterByCategory"] = categoryOfPost;
            ViewData["FilterByDate"] = dateOfPost?.ToString("yyyy-MM-dd");

            // Working with database
            var database = new DBServices(db);
            var userPosts = await database.GetUserPosts(pageNumber, titleOfPost, User.Identity.GetUserId(), categoryOfPost, dateOfPost);
            var categories = (await database.GetCategories()).Select(x => new SelectListItem { Value = x.Id, Text = x.Name });

            // Collection of the posts with categories
            var posts = new ListPosts
            {
                UserPosts = userPosts,
                Categories = categories
            };

            // Go to the view PersonalArea/Index
            return View(posts);
        }

        /* Show the image of the post */
        public async Task<IActionResult> Image(string id)
        {
            // Working with database
            var database = new DBServices(db);
            var post = await database.GetPost(id);

            // The view PersonalArea/Index
            return File(post.ImageData, post.ImageType);
        }
    }
}
