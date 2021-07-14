using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Configuration;

namespace Blog.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        /* Dependency Injection */
        private BlogDBContext db;
        private IConfiguration _config;

        public PostController(BlogDBContext _db, IConfiguration config)
        {
            db = _db;
            _config = config;
        }

        /* Create new post */
        public async Task<IActionResult> NewPost()
        {
            // Working with database
            var database = new DBServices(db);

            // All categories
            var categories = new NewPost();
            categories.Categories = (await database.GetCategories()).Select(x => new SelectListItem { Value = x.Id, Text = x.Name });

            // Go to the view Post/NewPost
            return View(categories);
        }

        /* Modify the post*/
        public async Task<IActionResult> EditPost(string id)
        {
            // Working with database
            var database = new DBServices(db);
            var post = await database.GetPost(id);

            // Check if user authorized to edit the post
            if (post.UserId != User.Identity.GetUserId()) return Unauthorized();

            // Post's files
            string pathToFolder = _config["PathToPostFiles"];
            string pathToSubFolder = post.Id;
            string folderName = Path.Combine(pathToFolder, pathToSubFolder);
            var namesOfFiles = new List<string>();
            int numberOfFiles = Directory.GetFiles(folderName).Length;
            for(var i=0; i<numberOfFiles; i++)
            {
                namesOfFiles.Add(Path.GetFileName(Directory.GetFiles(folderName)[i]));
            }

            // Old post
            var postFields = new EditPost()
            {
                Id = post.Id,
                Name = post.Name,
                Content = post.Content,
                Date = post.Date,
                Description = post.Description,
                FileNames = namesOfFiles               
            };

            // All categories
            postFields.Categories = (await database.GetCategories()).Select(x => new SelectListItem { Value = x.Id, Text = x.Name, Selected = x.Id == post.CategoryId });

            // Go to the view Post/EditPost
            return View(postFields);
        }

        /* Delete the post*/
        public async Task<IActionResult> DeletePost(string id)
        {
            // Working with database
            var database = new DBServices(db);
            var post = await database.GetPost(id);
            if (post.UserId != User.Identity.GetUserId()) return Unauthorized();

            // Delete folder with files
            string pathToFolder = _config["PathToPostFiles"];
            string pathToSubFolder = id;
            string path = Path.Combine(pathToFolder, pathToSubFolder);

            if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
            }

            await database.DeletePost(post);

            // Go to the view PersonalArea/Index
            return RedirectToAction("Index", "PersonalArea");
        }

        /* Modify post */
        public async Task<IActionResult> UpdatePost(EditPost modifiedPost)
        {
            // Working with database
            var database = new DBServices(db);
            var post = await database.GetPost(modifiedPost.Id);
            if (post.UserId != User.Identity.GetUserId()) return Unauthorized(); // Check if user authorized to edit the post

            // Update content, title, category, description
            post.Date = DateTime.UtcNow;
            post.Content = modifiedPost.Content;
            post.Name = modifiedPost.Name;
            post.CategoryId = modifiedPost.CategoryId;
            post.Description = modifiedPost.Description;

            // Update image
            if (modifiedPost.Image != null)
            {
                post.ImageName = modifiedPost.Image.FileName;
                post.ImageType = modifiedPost.Image.ContentType;
                using (var ms = new MemoryStream())
                {
                    modifiedPost.Image.CopyTo(ms);
                    post.ImageData = ms.ToArray();
                }
            }

            // Save Post's files to the folder
            if (modifiedPost.Files != null)
            {
                foreach (var file in modifiedPost.Files)
                {
                    string pathToFolder = _config["PathToPostFiles"];
                    string pathToSubFolder = post.Id;
                    string fileName = file.FileName;
                    string pathToFile = Path.Combine(pathToFolder, pathToSubFolder, fileName);

                    using (var stream = new FileStream(pathToFile, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                }
            }

            // Save changes
            await database.EditPost(post); // Save changes to db

            // Go to the view PersonalArea/Index
            return RedirectToAction("Index", "PersonalArea");
        }

        /* Create new post */
        public async Task<IActionResult> SavePost(NewPost newPost)
        {

            // Create folder with name files in wwwroot for user's files
            string pathTowwwroot = _config["PathToWwwroot"];
            string folderNameForFiles = "files";
            string folderFilesInWwwroot = Path.Combine(pathTowwwroot, folderNameForFiles);

            if(!Directory.Exists(folderFilesInWwwroot))
            {
                Directory.CreateDirectory(folderFilesInWwwroot);
            }

            // Working with database
            var database = new DBServices(db);

            if (ModelState.IsValid)
            {
                // Create post
                // Converting NewPost to Post, because NewPost contains IFormFile File, but EF can not save it directly 
                var post = new Post();
                post.Id = Guid.NewGuid().ToString();
                post.Date = DateTime.UtcNow;
                post.Content = newPost.Content;
                post.Name = newPost.Name;
                post.UserId = User.Identity.GetUserId();
                post.CategoryId = newPost.CategoryId;
                post.Description = newPost.Description;
                post.ImageName = newPost.Image.FileName;
                post.ImageType = newPost.Image.ContentType;
                using (var ms = new MemoryStream())
                {
                    newPost.Image.CopyTo(ms);
                    post.ImageData = ms.ToArray();
                }

                // Сreate folder for post's files
                string pathToFolder = _config["PathToPostFiles"];
                string pathToSubFolder = post.Id;
                string folderName = Path.Combine(pathToFolder, pathToSubFolder);

                if (!Directory.Exists(folderName))
                {
                    Directory.CreateDirectory(folderName);

                    // Save Post's files to the folder
                    if (newPost.Files != null)
                    {
                        foreach (var file in newPost.Files)
                        {
                            string fileName = file.FileName;
                            string pathToFile = Path.Combine(folderName, fileName);

                            using (var stream = new FileStream(pathToFile, FileMode.Create))
                            {
                                file.CopyTo(stream);                                
                            }
                        }
                    }
                }

                // Save post
                await database.AddNewPost(post);

                return RedirectToAction("Index", "PersonalArea"); // Save post and stay in personal area
            }

            // Get all categories
            var model = new NewPost();
            model.Categories = (await database.GetCategories())
                .Select(x => new SelectListItem { Value = x.Id, Text = x.Name });

            // Go to the view Post/NewPost
            return View("NewPost", model);
        }

        /* Delete post's file*/
        public async Task<IActionResult> DeleteFile(string postId, string fileName)
        {
            string pathToFolder = _config["PathToPostFiles"];
            string pathToSubFolder = postId;
            string nameOfFile = fileName;
            string pathToFile = Path.Combine(pathToFolder, pathToSubFolder, nameOfFile);

            FileInfo file = new FileInfo(pathToFile);
            file.Delete();

            return await Task.Run<IActionResult>(()=>
            {
                // Go to the view Post/EditPost
                return RedirectToAction("EditPost", "Post", new { id = postId });
            });
        }

        /* Add new category*/
        public async Task<IActionResult> NewCategory(NewPost nameOfCategory)
        {
            // Working with database
            var database = new DBServices(db);

            var category = new Category();
            category.Id = Guid.NewGuid().ToString();
            category.Name = nameOfCategory.CategoryName;

            await database.AddCategory(category);

            // Go to the view Post/NewPost
            return RedirectToAction("NewPost", "Post");
        }
    }
}
