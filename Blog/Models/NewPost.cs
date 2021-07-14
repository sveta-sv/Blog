using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Models
{
    /* View: Post/NewPost */
    public class NewPost
    {
        public string UserId { get; set; }

        [Required(ErrorMessage = "Please choose category")]
        public string CategoryId { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }

        [Required(ErrorMessage = "Please enter title of the post")]
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter some content of the post")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Please add image to the post")]
        public IFormFile Image { get; set; }

        public List<IFormFile> Files { get; set; }

        [Required(ErrorMessage = "Please enter short description of the post")]
        [StringLength(800, MinimumLength = 10)]
        public string Description { get; set; }

        [StringLength(25, MinimumLength = 1)]
        public string CategoryName { get; set; }
    }
}
